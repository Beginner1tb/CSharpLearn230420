----
'task.run(()=>{},token)'的解析
### CancellationToken 的作用
#### 1.传递取消请求：
1. CancellationToken 用于在不同的任务或线程之间传递取消请求。

2. 当你调用 CancellationTokenSource.Cancel() 时，所有使用该 CancellationToken 的任务都会收到取消请求。
 
#### 2. 与任务框架集成：
1. 当你将 CancellationToken 传递给 Task.Run 时，任务框架会注册一个回调，以便在取消请求发出时能够及时响应。
2. 如果任务在启动之前就收到取消请求，任务甚至可能不会开始执行，并直接以取消状态结束。

### ThrowIfCancellationRequested 的作用
#### 1. 显式检查取消状态：
1. ThrowIfCancellationRequested() 是 CancellationToken 的一个方法，用于在代码执行过程中显式地检查是否有取消请求。
2. 如果检测到取消请求，这个方法会抛出一个 OperationCanceledException，从而终止任务的执行。

#### 2. 终止任务执行：
1. 调用 ThrowIfCancellationRequested() 会在取消请求被发出时抛出异常，这通常用于在任务执行过程中定期检查和响应取消请求。