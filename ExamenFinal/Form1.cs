using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSGL12;

namespace ExamenFinal
{
    public partial class Form1 : Form
    {
        public Handler handler;
        private System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();
            handler = new Handler(); // manejador de eventos de OpenGL

            //Registrar los manejadores de eventos de OpenGL y sobreescrituras de paint, mouse, etc...
            csgL12Control1.OpenGLStarted += new CSGL12Control.DelegateOpenGLStarted(handler.OpenGLStarted);
            csgL12Control1.Paint += new PaintEventHandler(handler.Paint);

            //Timer para la animacion
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += new EventHandler(timerHandler);
            timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        void timerHandler(object sender, EventArgs e)
        {
            if (false == DesignMode)
            {
                csgL12Control1.Invalidate();
            }
        }

    }
}
