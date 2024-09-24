### 一. DotNetty使用（服务端）
#### 1. Bootstrap 配置步骤
(1) 配置事件循环组``EventLoopGroup``；

(2) 配置通道类型``Channel``
包含TCP，UDP，Epoll，HTTP之类

如果使用的是TCP，注意客户端用的是``TcpSocketChannel ``，服务端是``TcpServerSocketChannel``

(3) 配置通道处理器

(4) 启动连接，注意客户端和服务端不同
#### 2. TCP和UDP的不同（服务端）
TCP和UDP在服务端上有着诸多区别，使用时需要注意：

1. TCP的通道是``TcpServerSocketChannel``，UDP的通道是``SocketDatagramChannel``
2. TCP的引导bootstrap是``ServerBootstrap``，UDP的引导是``Bootstrap``
3. TCP的默认报文形式是``IByteBuffer``，UDP的默认报文形式是``DatagramPacket``
4. TCP的服务端是阻塞的，UDP是非阻塞的，需要注意阻塞的写法

#### 3. 多引导bootstraps
以TCP和UDP为例，可以写多个bootstrap，用以处理不同的任务，注意，如果TCP的阻塞方式不正确，则会影响其他的bootstrap的工作