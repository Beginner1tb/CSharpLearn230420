### 1. 递归获取文件夹和文件的信息
获取文件夹和文件信息，首先是获取文件夹的信息，遍历当前的文件夹，GetDirectories获取所有文件夹的路径，然后再一个一个进入子文件夹获取文件夹信息，直到当前层级下获取不到文件夹信息，此时再GetFiles获取当前文件的信息

当然，先获取文件信息也是可以的

注意，每获取到一个文件夹信息就要调用一次本方法，但是上一次的使用的字段还在内存里，并没有清除
### 2. 上传FTP
如果不使用异步Task或者方法，切忌使用``fileStream.CopyToAsync()``，会导致发送0字节的空文件

必须使用使用 ``await`` 关键字等待 ``CopyToAsync()`` 完成
