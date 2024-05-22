## WPF绑定学习
### 1. 数组绑定
#### 使用过程
1. 建立实现INofifyPropertyChanged的类，定义所需要数组，注意需要是ObservableCollection< T >泛型；
2. 建立PropertyChangedEventHandler事件用以处理属性变化；
3. 在数组属性内定义数据变化方法OnPropertyChanged并传递数组值；
4. 在上下文中绑定这个类的实例,xaml文件中可以直接绑定到xxxx[1]这个数组的特定项。
#### 注意事项
1. 只有ObservableCollection< T >实现了INofifyPropertyChanged接口，用别的T[]数组并不能实现绑定数据的修改
2. 绑定类可以用构造函数进行初始化，当然也可以在别的地方进行初始化，但是要注意对应的ObservableCollection< T >一定需要进行初始化，否则为空会报错；
----
### 2. 多个类的绑定
#### 使用过程
1. 建立一个更高阶的类，需要实现INofifyPropertyChanged接口，其中包含需要绑定的子类，子类依旧按普通绑定编写；
2. 跟上述操作相同，也要PropertyChangedEventHandler事件和OnPropertyChanged方法；
3. 在绑定过程中，只要绑定这个高阶类的实例就可以了
4. 具体绑定的属性需要写到具体的类中，比如原有的是TopClass,下属Middle1和Middle2，那么绑定路径就要写成Middle1.xxx，Middle2.xxx。
#### 注意事项
1. 绑定上下文有顺序，以绑定类为最高阶，依次向下类推
----
### 3.索引器
#### 使用过程
1. 目前索引器仅使用在数组上，用来简化数组的使用过程；
2. 定义属性T this[int index]，可以使用快捷指令indexer实现；
3. 这样数组就不需要建立实例的，可以由原有的Class.Array[1]写为Class[1];
4. 索引器还有其他用法
