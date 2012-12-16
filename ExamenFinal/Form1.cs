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
            csgL12Control1.KeyDown += new KeyEventHandler(handler.KeyDown);
            csgL12Control1.KeyUp += new KeyEventHandler(handler.KeyUp);

            //adueñarse de la captura de eventos
            csgL12Control1.Focus();
            //Permitir ver las teclas antes de consumirlas
            this.KeyPreview = true;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10; // 10 ms... suficiente para lograr hasta 70fps
            timer.Tick += new EventHandler(timerHandler);
            timer.Start();
     

        }
        void timerHandler(object sender, EventArgs e)
        {
            if (false == DesignMode)
            {
                csgL12Control1.Invalidate(); //obligar a redibujar!!!!
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
           //Preproceso de teclas para capturar teclas de control apropiadamente
        //pre: KeyPreview debe ser true
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if
                (
                   (keyData == Keys.Up)
                || (keyData == Keys.Down)
                || (keyData == Keys.Left)
                || (keyData == Keys.Right)
                )
            {
                KeyEventArgs e = new KeyEventArgs(keyData);

                if (true == csgL12Control1.Focused)
                {
                    handler.KeyDown(csgL12Control1, e);
                }
                else
                {
                    // The CSGL12Control does not have focus.
                    // Let's simply drop the dialog key event.  The user
                    // may have focus on a different control.
                }


                return (true);
            }

            return base.ProcessDialogKey(keyData);
        }
    }
    
}
