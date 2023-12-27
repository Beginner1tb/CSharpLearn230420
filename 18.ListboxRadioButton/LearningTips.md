1. ListBox这类含层级的控件，内部的事件，例如SelectionChanged,如果其内部还有别的控件，如Button,RadioButton，可能不会响应
2. 如果向给已有控件增加属性，需要使用到自定义控件，注意，不是用户控件。创建自定义控件在新建项--WPF--自定义控件。
3. 自定义控件通过属性依赖增加控件属性，当然，本身要继承原控件。
4. Page与Window不一样，注意Page一般使用d:DesignHeight和d:DesignWidth，表示设计时候显示的高宽，不是框死的高宽，实际大小根据引用Page的页面自动调整。
5. Frame和ContentControl控件不同，Frame控件作为导航控件，而ContentControl控件只是一个基本容器