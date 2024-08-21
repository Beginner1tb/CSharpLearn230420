using Polly;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace _28.FtpUploadTest1
{
    class Program
    {
        static readonly string folderPath = @"D:/temp/image/";
        static readonly string ftpUrl = "ftp://localhost/folder1";
        static readonly string ftpUsername = "user1";
        static readonly string ftpPassword = "1";
        static int successfulUploadCount = 0;
        static async Task Main(string[] args)
        {
            await RunScheduledUploadAsync();

            Console.ReadLine();

            // var filePaths = Directory.GetFiles(folderPath);
            //
            // List<Task> uploadTasks = new List<Task>();
            //
            // foreach (var filePath in filePaths)
            // {
            //     uploadTasks.Add(UploadFileAsync(filePath, ftpUrl, ftpUsername, ftpPassword));
            // }
            //
            // // 等待所有文件上传完成
            // await Task.WhenAll(uploadTasks);
        }
        private static async Task RunScheduledUploadAsync()
        {
            while (true)
            {
                //await UploadDirectoryAsync(folderPath,ftpUrl);

                //UploadDirectory(folderPath, ftpUrl, "",ftpUsername, ftpPassword);
                //await UploadFilesAsync();
                UploadDirectoryIncrementally(folderPath, ftpUrl, "", ftpUsername, ftpPassword);
                //TraverseDirectory(folderPath);
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
        /// <summary>
        /// 递归获取文件夹测试
        /// </summary>
        /// <param name="path"></param>
        static void TraverseDirectory(string path)
        {
            try
            {
                // 获取所有子文件夹
                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                {
                    Console.WriteLine("Directory: " + directory);
                    // 递归调用遍历子文件夹
                    TraverseDirectory(directory);
                }

                // 获取所有文件
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    Console.WriteLine("File: " + file);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Access denied to: " + path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        static void UploadDirectoryIncrementally(string localPath, string ftpHost, string ftpBasePath, string ftpUsername, string ftpPassword)
        {
            // 获取FTP目录的当前内容
            var existingEntries = GetFTPDirectoryContents(ftpHost + ftpBasePath, ftpUsername, ftpPassword);

            // 遍历所有文件
            foreach (string localFile in Directory.GetFiles(localPath))
            {
                string fileName = Path.GetFileName(localFile);
                string ftpFilePath = $"{ftpHost}{ftpBasePath}/{fileName}";
                if (!existingEntries.ContainsKey(fileName.ToLower()))
                {
                    UploadFile(localFile, ftpFilePath, ftpUsername, ftpPassword);
                }
            }

            // 遍历所有目录
            foreach (string localDir in Directory.GetDirectories(localPath))
            {
                string dirName = Path.GetFileName(localDir);
                string ftpDirPath = ftpBasePath + "/" + dirName;
                if (!existingEntries.ContainsKey(dirName.ToLower()))
                {
                    CreateFTPDirectory(ftpHost + ftpDirPath, ftpUsername, ftpPassword);
                }
                // 递归调用以处理子目录
                UploadDirectoryIncrementally(localDir, ftpHost, ftpDirPath, ftpUsername, ftpPassword);
            }
        }

        static Dictionary<string, bool> GetFTPDirectoryContents(string ftpPath, string ftpUsername, string ftpPassword)
        {
            Dictionary<string, bool> entries = new Dictionary<string, bool>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 0)
                        {
                            var isDirectory = line.Contains("<DIR>") || line.ToLower().Contains("dir");
                            var name = parts[parts.Length - 1];
                            entries[name.ToLower()] = isDirectory;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle error, possibly no files in directory
                Console.WriteLine($"Error accessing FTP: {ex.Status}");
            }

            return entries;
        }
        static void CreateFTPDirectory(string directoryPath, string ftpUsername, string ftpPassword)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Directory created successfully: {directoryPath}");
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Console.WriteLine($"Failed to create directory {directoryPath}: {response.StatusDescription}");
                }
            }
        }

        static void UploadFile(string localFile, string ftpFilePath, string ftpUsername, string ftpPassword)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFilePath);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
            request.UseBinary = true;
            using (Stream requestStream = request.GetRequestStream())
            using (FileStream fileStream = File.Open(localFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileStream.CopyTo(requestStream);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == FtpStatusCode.ClosingData)
                {
                    Console.WriteLine($"文件 {localFile} 上传成功");
                    successfulUploadCount++;
                    Console.WriteLine($"Status description: {response.StatusDescription}");
                    Console.WriteLine($"上传文件一共{successfulUploadCount}个");
                }
                else
                {
                    Console.WriteLine("Upload failed.");
                    Console.WriteLine($"Status code: {response.StatusCode}");
                    Console.WriteLine($"Status description: {response.StatusDescription}");
                }
            }


            // // 读取本地文件内容
            // byte[] fileContents;
            // using (StreamReader sourceStream = new StreamReader(localFile))
            // {
            //     fileContents = System.Text.Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            // }
            //
            // request.ContentLength = fileContents.Length;
            //
            // // 上传文件
            // using (Stream requestStream = request.GetRequestStream())
            // {
            //     requestStream.Write(fileContents, 0, fileContents.Length);
            // }
            //
            // using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            // {
            //     Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            //     successfulUploadCount++;
            //     Console.WriteLine($"上传文件一共{successfulUploadCount}个");
            // }


        }
        #region 以下未使用
        private static async Task UploadDirectoryAsync(string localPath, string ftpPath)
        {
            var files = Directory.GetFiles(localPath);

            // 递归遍历并上传每个文件
            foreach (var file in files)
            {
                string filename = file.Replace("\\", "/");
                // 计算文件在FTP服务器上的目标路径
                string relativePath = Path.GetRelativePath(localPath, file);
                string ftpFileUrl = $"{(ftpPath + "/" + relativePath).Replace("\\", "/")}";
                //string ftpFileUrl = $"{ftpPath}/{relativePath}";
                // 确保FTP上存在相应的目录结构

                //不能用文件形式
                //string ftpDirectoryUrl = Path.GetDirectoryName(ftpFileUrl);
                string ftpDirectoryUrl = ftpFileUrl.Substring(0, ftpFileUrl.LastIndexOf('/'));
                await EnsureFtpDirectoryExistsAsync(ftpDirectoryUrl);

                // 上传文件
                bool uploadSuccess = await UploadFileWithRetryAsync(filename, ftpFileUrl);
                if (uploadSuccess)
                {
                    // successfulUploadCount++;
                    // Console.WriteLine($"当前成功上传的文件总数：{successfulUploadCount}");
                }
                Console.WriteLine($"当前成功上传的文件总数：{successfulUploadCount}");
            }

            // 递归遍历每个子目录
            var directories = Directory.GetDirectories(localPath);
            foreach (var directory in directories)
            {
                string directoryName = Path.GetFileName(directory);
                string newFtpPath = $"{(ftpPath + directoryName).Replace("\\", "/")}";
                await UploadDirectoryAsync(directory, newFtpPath);
            }
        }
        private static async Task EnsureFtpDirectoryExistsAsync(string ftpDirectoryUrl)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpDirectoryUrl);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    Console.WriteLine($"目录 {ftpDirectoryUrl} 已创建");
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Console.WriteLine($"创建目录 {ftpDirectoryUrl} 失败: {ex.Message}");
                }
            }
        }
        private static async Task UploadFilesAsync()
        {
            var filePaths = Directory.GetFiles(folderPath);

            foreach (var filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath).Replace("\\", "/");
                string ftpFileUrl = $"{ftpUrl}{fileName}";

                if (await FileExistsOnFtpAsync(ftpFileUrl))
                {
                    Console.WriteLine($"文件 {fileName} 已存在，跳过上传。");
                }
                else
                {
                    bool uploadSuccess = await UploadFileWithRetryAsync(filePath, ftpFileUrl);
                    if (uploadSuccess)
                    {

                        successfulUploadCount++;
                        Console.WriteLine($"当前成功上传的文件总数：{successfulUploadCount}");
                    }
                }
            }
        }
        private static async Task<bool> UploadFileWithRetryAsync(string filePath, string ftpFileUrl)
        {
            var retryPolicy = Policy
                .Handle<Exception>() // 处理所有异常
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2), (exception, timeSpan, retryCount, context) =>
                {
                    // 在每次重试之前执行的逻辑（例如日志记录）
                    Console.WriteLine($"尝试上传文件 {filePath} 失败，正在进行第 {retryCount} 次重试...");
                });

            try
            {
                bool uploadSuccess = false;
                await retryPolicy.ExecuteAsync(async () =>
                {
                    string ftpDirectoryUrl = ftpFileUrl.Substring(0, ftpFileUrl.LastIndexOf('/'));

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFileUrl);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    var filePaths = Directory.GetFiles(folderPath + "90度");

                    foreach (var filePath in filePaths)
                    {

                        string fileName = Path.GetFileName(filePath).Replace("\\", "/");


                        string ftpFileUrl1 = $"{ftpDirectoryUrl}/{fileName}";
                        Console.WriteLine(ftpFileUrl1);
                        if (await FileExistsOnFtpAsync(ftpFileUrl1))
                        {
                            Console.WriteLine($"文件 {fileName} 已存在，跳过上传。");
                        }
                        else
                        {
                            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                using (Stream ftpStream = await request.GetRequestStreamAsync())
                                {
                                    await fileStream.CopyToAsync(ftpStream);
                                }

                            }

                            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                            {
                                if (response.StatusCode == FtpStatusCode.ClosingData)
                                {
                                    Console.WriteLine($"文件 {filePath} 上传成功");
                                    successfulUploadCount++;
                                    Console.WriteLine($"Status description: {response.StatusDescription}");
                                }
                                else
                                {
                                    Console.WriteLine("Upload failed.");
                                    Console.WriteLine($"Status code: {response.StatusCode}");
                                    Console.WriteLine($"Status description: {response.StatusDescription}");
                                }
                            }
                            // byte[] fileContents = await File.ReadAllBytesAsync(filePath);
                            // request.ContentLength = fileContents.Length;
                            //
                            // using (Stream requestStream = await request.GetRequestStreamAsync())
                            // {
                            //     await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
                            // }
                            //
                            // using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                            // {
                            //     if (response.StatusCode != FtpStatusCode.ClosingData)
                            //     {
                            //         throw new Exception($"上传文件 {filePath} 失败，响应状态码: {response.StatusCode}");
                            //     }
                            //
                            //     Console.WriteLine($"文件 {filePath} 上传成功");
                            //     successfulUploadCount++;
                            //     
                            // }
                        }
                    }

                    // request.UseBinary = true;
                    //
                    //  byte[] fileContents = await File.ReadAllBytesAsync(filePath);
                    //  request.ContentLength = fileContents.Length;
                    //
                    //  using (Stream requestStream = await request.GetRequestStreamAsync())
                    //  {
                    //      await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
                    //  }
                    //
                    //  using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                    //  {
                    //      if (response.StatusCode != FtpStatusCode.ClosingData)
                    //      {
                    //          throw new Exception($"上传文件 {filePath} 失败，响应状态码: {response.StatusCode}");
                    //      }
                    //
                    //      Console.WriteLine($"文件 {filePath} 上传成功");
                    //  }
                });
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"上传文件 {filePath} 失败，错误信息：{ex.Message}");
                return false; // 上传失败
            }
        }
        private static async Task<bool> FileExistsOnFtpAsync(string ftpFileUrl)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFileUrl);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    return response.StatusCode == FtpStatusCode.FileStatus;
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false; // 文件不存在
                }
                else
                {
                    Console.WriteLine($"检查文件是否存在时发生错误: {ex.Message}");
                    return false;
                }
            }
        }
        static async Task UploadFileAsync(string filePath, string ftpUrl, string ftpUsername, string ftpPassword)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + Path.GetFileName(filePath));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (Stream ftpStream = await request.GetRequestStreamAsync())
                    {
                        await fileStream.CopyToAsync(ftpStream);
                    }

                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == FtpStatusCode.ClosingData)
                    {
                        Console.WriteLine("Upload successful.");
                        Console.WriteLine($"Status description: {response.StatusDescription}");
                    }
                    else
                    {
                        Console.WriteLine("Upload failed.");
                        Console.WriteLine($"Status code: {response.StatusCode}");
                        Console.WriteLine($"Status description: {response.StatusDescription}");
                    }
                }

                // using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                // {
                //     Console.WriteLine("文件大小: " + response.ContentLength);
                // }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    Console.WriteLine("文件不存在"); // 文件不存在
                }
                else
                {
                    Console.WriteLine($"检查文件是否存在时发生错误: {ex.Message}");

                }
            }
        }
        #endregion

    }
}
