using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Welcome
{
    public partial class Welcome : Form
    {
       
        private delegate void RecDelegate(string str); //委托
        private delegate void AnimationDelegate(int Width, int Height); //委托
        private delegate void StartintendDelegate(); //委托
        private static Welcome static_Welcome;
        private string strZhongdaxia = @"                        .-'      '-. 
                       /            \ 
                      |              | 
                      |,  .-.  .-.  ,| 
                      | )(__/  \__)( | 
                      |/     /\     \| 
            (@_       (_     ^^     _) 
 _           ) \_______\__|IIIIII|__/__________________________ 
(_)Zhongdaxia{}<________|-\IIIIII/-|___________________________> 
             )_/        \          / 
            (@           `--------`



                                          --- 为中华之崛起而编程";

#region 基本数据
        int screenwidth;  //屏幕宽度
        int screenheight; //屏幕高度

       
#endregion


        public Welcome()
        {
            InitializeComponent();
            /// <summary>
            /// 对话框基本属性设置
            /// </summary>
            #region 对话框基本属性
            this.FormBorderStyle = FormBorderStyle.None;       //对话框样式
            this.BackColor = System.Drawing.Color.Black;       //背景色
            this.TransparencyKey = System.Drawing.Color.Red;   //背景透明色
            this.ForeColor = System.Drawing.Color.Lime;        //控件颜色
            this.TopMost = true;                               //置顶
            #endregion

            static_Welcome = this;
            SetFontSize();

            Thread prinft = new Thread(Zhongdaxia);
            prinft.Start();
        }


        /// <summary>
        ///  计算字体大小
        /// </summary>
        public void SetFontSize()
        {
            /// <summary>
            /// 27      1920 * 1080   [32]     22寸以上的16：9宽屏
            /// 27      1680 * 1050   [27]     21~23寸16：10显示器
            /// 27      1280 * 1024   [22]     17~20寸之间的台式
            /// 27      1366 * 768    [22]     主流笔记本
            /// </summary>

            //分辨率
            screenwidth = System.Windows.Forms.SystemInformation.WorkingArea.Width;
            screenheight = System.Windows.Forms.SystemInformation.WorkingArea.Height;

            int fontsize = 18;
            if ((screenwidth > 1920))
            {
                fontsize = 35;   //1920 * 1080
            }
            if ((screenwidth <= 1920) && (screenwidth > 1400))
            {
                fontsize = 28;   //1920 * 1080
            }
            if ((screenwidth <= 1400) && (screenwidth > 1000))
            {
                fontsize = 22;   //1920 * 1080
            }
            if ((screenwidth <= 1000))
            {
                fontsize = 16;   //1920 * 1080
            }
            ////设置字体
            Font myFont = new Font(new FontFamily("宋体"), fontsize);
            this.Font = myFont;

            //只能先隐藏的生成一边获取label的宽高了
            label1.Visible = false;
            label1.Text = strZhongdaxia;
            //生成后的宽高
            int lablewidth = label1.Width;
            int Labelheight = label1.Height;
            //再置空
            label1.Text = "";
            label1.Visible = true;

            //全屏
            this.WindowState = FormWindowState.Maximized;

            //控件的相对X,Y位置
            int xmove = (int)(lablewidth * 0.05);
            int lablex = (screenwidth - lablewidth) / 2 + xmove;
            int labley = (screenheight - Labelheight) / 2;
 
            //设置控件位置
            label1.SetBounds(lablex, labley, lablewidth, Labelheight);
        }

        /// <summary>
        /// 绘制线程
        /// </summary>
        private void Zhongdaxia()
        {

            int speed = 2;
            for (int i = 0; i < strZhongdaxia.Length; i++)
            {
                fresh("" + strZhongdaxia[i]);
                if (strZhongdaxia[i] == 'Z')
                {

                    speed = 50;
                }
                if (strZhongdaxia[i] == '{')
                {

                    speed = 1;
                }
                if (strZhongdaxia[i] == '为')
                {

                    speed = 200;
                }
                if (strZhongdaxia[i] == '之')
                {

                    speed = 100;
                }
                Thread.Sleep(speed);
            }
            Thread.Sleep(1000);
           
            #region 关闭动画
            Thread animation = new Thread(animation_retraction_thread);
            animation.Start();
            #endregion

        }

        /// <summary>
        ///  界面交互刷新方法
        /// </summary>
        private void fresh(string str)
        {
            if (Welcome.static_Welcome.label1.InvokeRequired)
            {
                RecDelegate fc = new RecDelegate(fresh);
                Welcome.static_Welcome.Invoke(fc, str);
            }
            else
            {
                Welcome.static_Welcome.label1.Text = Welcome.static_Welcome.label1.Text + str;
            }
        }



        /// <summary>
        /// 动画  - 回缩
        /// </summary>
        private void animation_retraction_thread()
        {

            /// <summary>
            /// 设置参数
            int speed = 2;            //回缩速度
            int singlesize = 20;       //每次回复基准   以横向宽度x为标准
            /// </summary>
         
            int RectangleWidth;    //回缩时矩形的宽度
            int RectangleHeight;   //回缩时矩形的高度

            float scale;//宽高比
            if (screenwidth > screenheight)
            {
                scale = (float)screenheight / (float)screenwidth;
            }
            else
                scale = screenwidth / screenheight;

            int singlesizex = singlesize;
            int singlesizey = Convert.ToInt32(singlesize * scale);

            RectangleWidth = screenwidth;
            RectangleHeight = screenheight;


            for (int i = 0; (RectangleWidth > 0 || RectangleHeight > 0); i++)
            {
                closeanimation(RectangleWidth, RectangleHeight);
                if (RectangleWidth > 0)
                    RectangleWidth = RectangleWidth - singlesizex;
                if (RectangleHeight > 0)
                    RectangleHeight = RectangleHeight - singlesizey;
                Thread.Sleep(speed);
            }
            /// <summary>
            ///  动画结束，添加事件，跳转事件
            ///  </summary>
            startintend();

        }


        //
        private void closeanimation(int Width, int Height)
        {
            if (Welcome.static_Welcome.InvokeRequired)
            {
                AnimationDelegate fc = new AnimationDelegate(closeanimation);
                Welcome.static_Welcome.Invoke(fc, Width, Height);
            }
            else
            {
                int x = (screenwidth - Width) / 2;
                int y = (screenheight - Height) / 2;

                System.Drawing.Drawing2D.GraphicsPath shape = new System.Drawing.Drawing2D.GraphicsPath();    //Gdi+
                Rectangle Rectangle = new Rectangle(x, y, Width, Height);
                shape.AddRectangle(Rectangle);     //绘制矩形窗口
                Welcome.static_Welcome.Region = new System.Drawing.Region(shape);
            }
            
        }


        //最后的跳转 欢迎界面结束
        private void startintend()
        {
            if (Welcome.static_Welcome.InvokeRequired)
            {
                StartintendDelegate fc = new StartintendDelegate(startintend);
                Welcome.static_Welcome.Invoke(fc);
            }
            else
            {
                Welcome.static_Welcome.Close();   //关闭欢迎界面
            }
        }
    }

}
