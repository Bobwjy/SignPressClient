using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;


namespace SignPressClient.Tools
{
    public partial class CartoonForm
    {
        private Form m_window;          //  操作的form窗体


        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;
        static int x;
        static int y;
        static int WIDTH;
        static int HEIGHT;
        int SH;
        int SW;
        public CartoonForm(Form window)
        {
            this.m_window = window;
            //（句柄，毫秒时间，函数）
            AnimateWindow(this.m_window.Handle, 100, AW_SLIDE + AW_VER_NEGATIVE);//开始窗体动画
            
            x = this.m_window.Location.X;//窗体左上角x坐标
            y = this.m_window.Location.Y;//窗体左上角y坐标
            
            WIDTH = this.m_window.Width;//得到当前窗体的宽度
            HEIGHT = this.m_window.Height;//得到当前窗体的高度
            
            //得到屏幕的分辨率
            SH = Screen.PrimaryScreen.Bounds.Height;
            SW = Screen.PrimaryScreen.Bounds.Width;

        }

        //重写API函数，用来执行窗体动画显示操作
        [DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);


        //结束窗体时特效
        public void CartoonClose()
        {

            while (this.m_window.Width > 124)
            {
                if (this.m_window.Height >= 40)
                {
                    this.m_window.Location = new System.Drawing.Point(x, y += 15);//设置窗体位置
                    this.m_window.Size = new Size(this.m_window.Width, this.m_window.Height -= 25);//设置窗体大小
                    this.m_window.Refresh();//重绘窗体
                }

                else
                {
                    this.m_window.Location = new System.Drawing.Point(x += 15, y);

                    if (this.m_window.Width > 123)
                    {

                        this.m_window.Size = new Size(this.m_window.Width -= 25, this.m_window.Height);
                        this.m_window.Refresh();
                        this.m_window.Opacity -= 0.04;//设置窗体透明度递减
                    }

                }
                Thread.Sleep(10);  // 线程休眠10毫秒
            }

        }


        //结束窗体时特效
        public void CartoonShowNormal()
        {
            this.m_window.Location = new System.Drawing.Point(x, y);//设置窗体位置
            this.m_window.Size = new Size(CartoonForm.WIDTH, CartoonForm.HEIGHT);//设置窗体大小
            this.m_window.Refresh();//重绘窗体

        }

        //拖动窗体时动态获得窗体的位置
        private void CartoonMove(object sender, EventArgs e)
        {
            x = this.m_window.Location.X;
            y = this.m_window.Location.Y;
        }
    }
}
