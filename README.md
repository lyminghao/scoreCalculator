# scoreCalculator 软件使用说明

## 1 简介
本软件可以为 `东北师范大学（NENU）` 本科生提供个性化成绩分析和计算服务，主要功能为依据本专业课程计划和个人已修成绩，生成`《毕业计算报告》`。用户可利用此报告评估自己各类别课程的修读情况，以及帮助判断成绩是否达到毕业要求。

本软件在`Windows`操作系统中运行，需要安装`.NET Framework 4.5`。

## 2 功能
本软件的主要功能有：

- 导入成绩：软件可以通过三种方式将个人成绩导入，分别是：
	- 网络抓取：在软件主界面的输入框中输入`教务系统用户名和密码`，再点击`抓取成绩`，即可将教务系统中的个人成绩档案一次性导入到软件中。
	
	- 文件导入：在软件主界面点击`文件导入`，并选择格式正确的文件，即可将文件中的成绩记录一次性导入到软件中。
	
	- 手动添加：在软件主界面点击`手动添加`，在弹出的表单中正确填写各项内容，再点击`添加`即可将一条成绩记录导入到软件中。

  \* 注：软件目前暂不支持删除已导入的成绩记录。如已经添加了错误的成绩记录，只能点击软件主界面的`清空数据`或重新启动软件，再重新导入所有数据，因此请小心操作。

- 毕业计算：软件将依据已经导入的成绩记录，结合课程计划文件，计算生成`《毕业计算报告》`。在软件主界面点击`毕业计算`即可执行该功能。

## 3 组成
软件以rar压缩包的形式发布，文件名为`scoreCalculator.rar`，解压后得到文件夹`scoreCalculator`，软件的所有组件都存在于该文件夹中：

- 可执行程序`scoreCalculator.exe`，是软件的主体，直接双击即可运行，如遇杀毒软件报毒请点击信任，否则软件将无法运行。

- 课程计划文件`Rules.txt`，用来描述某专业的课程计划。它按照一定格式编写（在 5.3 节详细说明），可以用`记事本`等文本编辑器打开和修改。它应该与可执行程序在同一目录下，如找不到文件或格式错误，会导致`毕业计算`功能无法使用。
  
  \* 注：由于作者对其他专业情况不够了解，故随软件发布的是`计算机科学与技术`专业`2011版`课程计划，适用范围是2014级之前的计科和中美班。同时欢迎有能力的同学共同发布其它专业的课程计划文件。

- 《毕业计算报告》文件`BIYE_Report.txt`：该文件用来存放生成的《毕业计算报告》。每次点击`毕业计算`后，该文件都会被清空重写。如需长期保存某次生成的《毕业计算报告》，请及时将文件复制到其它目录。

- 导入示例文件`input.txt`：该文件是`文件导入`功能的示例，供不熟悉该功能的用户查看软件接受的文件格式。在使用该功能时，也可以选择导入格式正确的其它文件。

## 4 导入成绩
### 4.1 网络抓取
在软件主界面的输入框中输入`教务系统用户名和密码`，再点击`抓取成绩`即可将教务系统中的个人成绩档案一次性导入到软件中。
可能的错误原因：

- 如果未连接到互联网或教务系统服务端宕机，则点击`抓取成绩`后会有对话框提示`抓取失败！网络请求超时。`此时应检查自己电脑的网络连接，或更换网络环境再次尝试。

- 如果输入的用户名或密码错误，导致请求被教务系统拒绝，则点击`抓取成绩`后会有对话框提示`抓取失败！用户名或密码错误。`此时应填入正确的用户名和密码并再次尝试。

- 如果软件在执行中产生了其它异常，会有对话框提示`抓取失败！未定义的错误。`导致此类错误的原因比较复杂，建议更换计算机或网络环境并再次尝试。

由于网络请求的不稳定性，如果长期无法使用该方法，建议采用更稳定的`文件导入`或`手动添加`方式。导入成功后，将有对话框显示本次导入的记录条数。

### 4.2 文件导入
提前准备好后缀名为`txt`的文本文件，按照：

> [课程名称] [学分] [成绩] [难度系数]

的格式，每行填写一条成绩记录。`课程名称`、`学分`和`成绩`的格式要求与 4.3 节相同。`难度系数`必须写`1.0`或`1.2`，请不要更改书写方式，否则将导致格式错误。

准备好文件后，在软件主界面上点击`文件导入`，在弹出的文件选择器中选择目标文件，再点击`打开`即可。`文件格式不正确`可能会导致失败。如果导入成功，将有对话框显示本次导入的记录条数。

### 4.3 手动添加
在软件主界面点击`手动添加`，会弹出添加表单，含有以下几项：

- 课程名称：该门课程的名称，不能为空，也不能与已有课程名称相同。

- 学分：该门课程的学分，必须为 `0.0 - 20.0` 的小数。

- 成绩：该门课程的成绩，必须为 `0 - 100` 的整数。

- 难度系数：根据学校相关规定，所有课程难度系数均为`1.0`或`1.2`，一般认为2011版课程计划中编码为 400 - 599 的课程为`1.2`，其它课程为`1.0`。详细情况请询问学院教务秘书。

该方式每次可以添加一门课程的成绩。填写完以上内容后，点击`添加`即可将成绩录入软件。如果添加失败，会有对话框提示具体原因。如果添加成功，也会有对话框提示。点击`重置`可以清空表单中已填写的内容。

### 4.4 导入范围
已导入的成绩记录是进行毕业计算的重要依据，以下是通过各种方式能够导入的成绩范围：

- 网络抓取：可获取个人成绩档案中的所有成绩。如果同一课程重修过，则只会导入一条最高分记录，无法体现重修次数。选过的课程无论及格与否都会导入一条记录。

- 文件读取：相当于批量进行的`手动添加`，可以一次性导入多条成绩记录。不受网络影响，较为稳定。

- 手动添加：可作为对已导入记录的少量补充，使用较为灵活。

## 5 毕业计算
### 5.1 计算项目
毕业计算是本软件的核心功能。软件主要计算以下项目，并在生成的`《毕业计算报告》`中体现：

- 已修课程列表：分为`通识教育必修课`、`通识教育选修课`、`专业教育必修课`和`专业教育选修课`四个类别列出已修课程。同一课程重修多次且已及格的，算作一门课程；尚未及格的课程不算已修课程。

  \* 注：软件划定课程类别的唯一依据是与软件同目录下的课程计划文件`Rules.txt`，其格式在5.3节详细说明。不同专业甚至不同年级的课程计划都可能有区别，在使用本功能时，请确保加载正确的课程计划文件。

- 毕业学分统计：按照学校规定，只有各类别分别达到课程计划要求的学分才能毕业。分为`通识教育必修课`、`通识教育选修课`、`专业教育必修课`、`专业教育选修课`和`生涯规划课程`五个类别报告已修学分数。同一课程重修多次且已及格的，只算一次学分；尚未及格的课程不计入已修学分。
  
  \* 注1：`专业教育必修课`是指课程计划中的`专业教育基础课`、`专业教育主干课`和`毕业论文/毕业设计`等本专业必修内容，`专业教育选修课`是指课程计划中的`专业教育系列课`等本专业选修内容。部分师范类专业有`教师教育必修课`和`教师教育选修课`，建议将相关课程和学分计入专必和专选类别中，即可与本软件兼容。
  
  \* 注2：按照学校规定，`生涯规划课程`可以由校内开设的`所有课程`充当，本软件将优先满足之前四个类别，再将各类别超出要求的学分计入`生涯规划课程`。据悉，个别学院对可作为`生涯规划课程`的课程进行了限定，软件不进行相关支持，请以学院教务秘书通知为准。

- 毕业绩点统计：按照学校规定，取得学位证应在达到毕业要求的基础上满足`平均学分绩点达到2.0`这一条件。软件将计算所有`及格课程`的平均学分绩点，同一课程重修多次且已及格的，取成绩最高的一次参与绩点计算。
  
  单科绩点计算公式：
  
  <img src="http://chart.googleapis.com/chart?cht=tx&chl= g_i = (\frac{s_i - 60}{10} + 1) * c_i * d_i" style="border:none;">
  $$ g_i = (\frac{s_i - 60}{10} + 1) * c_i * d_i $$
  
  其中$g_i$为及格课程的单科绩点，$s_i$为课程成绩，$c_i$为课程学分，$d_i$为课程难度系数。
  
  平均学分绩点计算公式：
  
  <img src="http://chart.googleapis.com/chart?cht=tx&chl= \overline{gpa} = \frac{\sum{g_i}}{\sum{c_i}}" style="border:none;">
  $$ \overline{gpa} = \frac{\sum{g_i}}{\sum{c_i}} $$
  
  其中$\overline{gpa}$为平均学分绩点，$g_i$和$c_i$分别为所有及格课程的单科绩点和学分。

- 学分学费计算：按照学校规定，选课总学分超出课程计划规定数量的学生，毕业时需缴纳`学分学费`。软件在课程计划规定的毕业学分基础上，再增加`免费学分11分`作为限额。如选课总学分超出该限额，将按照超出部分`每学分80元`计算学分学费。需要注意的是，`挂科后通过补考`由于只进行了一次选课，故不会重复计入选课总学分。
  
  \* 注：由于重修涉及的逻辑较为复杂，本软件`不支持同一课程名称多次导入`。如同一课程重修多次，请选择导入成绩最高的一次。如需通过重修记录计算学分学费，建议在导入完成后`手动添加`这样一条记录：
  > 重修课 [重修总学分] 0 1.0
  
  即可把重修总学分计入学分学费，也不影响正常的已修课程和平均绩点计算。

### 5.2 报告格式
生成的《毕业计算报告》（文件名：`BIYE_Report.txt`）将按照以下格式组织：
> 
> 毕业计算报告
> 
> \=\=\=\=\=\=\=\=\=\=\=\=
> 
> 1 已修课程列表
> 
> 1.1 通识教育必修课，匹配到以下课程：
> 
> [课程名称列表...]
> 
> 1.2 通识教育选修课，匹配到以下课程：
> 
> [课程名称列表...]
> 
> 1.3 专业教育必修课，匹配到以下课程：
> 
> [课程名称列表...]
> 
> 1.4 专业教育选修课，匹配到以下课程：
> [课程名称列表...]
> 
> \=\=\=\=\=\=\=\=\=\=\=\=
> 
> 2 毕业学分统计
> 
> 毕业要求总学分 [x] 分，已修课程学分 [y] 分。
> 
> 2.1 通识教育必修课，要求 [x] 分，已修 [y] 分。
> 
> 还需要修以下课程：
> 
> [课程名称列表...]
> 
> 2.2 通识教育选修课，要求 [x] 分，已修 [y] 分。
> 
> 还需要修 [z] 分。
> 
> 注意：课程计划规定，各类型通选课均需达到一定学分。软件不能辨别课程类型，请根据 1.2 中的已修列表判断是否符合。
> 
> 2.3 专业教育必修课，要求 [x] 分，已修 [y] 分。
> 
> 还需要修以下课程：
> 
> [课程名称列表...]
> 
> 2.4 专业教育选修课，要求 [x] 分，已修 [y] 分。
> 
> 还需要修 [z] 分。
> 
> 2.5 生涯规划课程，要求 [x] 分，已修 [y] 分。
> 
> 还需要修 [z] 分。
> 
> 注意：生涯规划课程的已修学分，是前四个类别超出各自要求的学分之和。
> 
> \=\=\=\=\=\=\=\=\=\=\=\=
> 
> 3 毕业绩点统计
> 
> 及格课程平均学分绩点为 [x] ，领取学位证要求达到 2.0 。
> 
> \=\=\=\=\=\=\=\=\=\=\=\=
> 
> 4 学分学费计算
> 
> 毕业时总学分超出 [x] 分的部分将收取学分学费（按毕业要求学分 + 11免费学分计算）。
> 
> 已选课程总学分 [y] 分，需缴纳学分学费 [z] 元（按每学分80元计算）。
> 
> \=\=\=\=\=\=\=\=\=\=\=\=
> 
> 欢迎使用。作者： @lyminghao

### 5.3 课程计划文件格式
课程计划文件（文件名：`Rules.txt`）将按照以下格式组织：
> \# [注释行，建议描述该文件对应课程计划版本、适用专业以及作者等信息]
> 
> s [通必学分] [通选学分] [专必学分] [专选学分] [生涯规划学分]
> 
> t 通识教育必修课
> 
> c [通必课程名称列表...]
> 
> t 专业教育必修课
> 
> c [专必课程名称列表...]
> 
> t 专业教育选修课
> 
> c [专选课程名称列表...]

\* 注：课程计划文件中不应含有`通识教育选修课`类别，在毕业计算时不属于以上三个类别的已修课程均被视为`通识教育选修课`。同一课程名称不能在文件中出现多次，否则会导致读取失败。

## 6 其它

### 6.1 免责声明
本软件是为了方便`东北师范大学(NENU)`本科生进行成绩管理和毕业资格测算而开发的民间软件，其中使用的一切`参数、公式和算法`均为作者根据对学校相关规定的理解而自行实现，如计算结果与学校教务处、学院教务秘书等官方计算结果有差异，`以官方计算结果为准`。因相信本软件计算结果而导致的任何后果，均由使用者自行承担，软件作者恕不负责。

### 6.2 使用许可
本软件将在`GitHub`公开包括源代码在内的整个项目文件，任何人都可以自由查看、复制、修改、传播和运行源代码，而无需通知作者，但不能将任何项目相关文件直接或修改后用于商业目的。

### 6.3 联系方式
如在使用过程中遇到问题，或对软件有任何改进建议，欢迎与作者联系。
联系方式：`lyminghao@qq.com`
