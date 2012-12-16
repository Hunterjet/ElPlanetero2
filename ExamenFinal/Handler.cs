using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using CSGL12;

namespace ExamenFinal
{
    public class Handler
    {

        public Handler()
        {
        }

        public void OpenGLStarted(CSGL12Control csgl12Control)
        {
            GL gl = csgl12Control.GetGL();

            if (null == gl) { return; }

            // evitar parpadeos
            if (true == gl.bwglSwapIntervalEXT)
            {
                gl.wglSwapIntervalEXT(1);
            }
        }


        public void Paint(object sender, PaintEventArgs e)
        {
            if (null == sender) { return; }
            if (false == (sender is CSGL12Control)) { return; }

            //Sacar el control de GL y sus dimensiones
            CSGL12Control csgl12Control = (sender as CSGL12Control);
            GL gl = csgl12Control.GetGL();

            int clientWidth = csgl12Control.ClientRectangle.Width;
            int clientHeight = csgl12Control.ClientRectangle.Height;

            if (clientWidth <= 0)
            {
                clientWidth = 1;
            }

            if (clientHeight <= 0)
            {
                clientHeight = 1;
            }

            //Asignar un viewport 
            gl.glViewport(0, 0, clientWidth, clientHeight);

            //Limpiar la pantalla con un color de fondo
            gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            //Asignar la vista del modelo
            gl.glMatrixMode(GL.GL_PROJECTION);
            gl.glLoadIdentity();
            double aspectRatio = 1.0;

            if (0 != clientHeight)
            {
                aspectRatio = ((double)(clientWidth) / (double)(clientHeight));
            }

            double verticalFieldOfViewAngle = 45.0;

            gl.gluPerspective
            (
                verticalFieldOfViewAngle, // Field of view angle (Y angle; degrees)
                aspectRatio, // width/height
                0.1, // distance to near clipping plane
                64000.0 // distance to far clipping plane
            );

            gl.glMatrixMode(GL.GL_MODELVIEW);

            gl.glLoadIdentity();
            //aqui va tu dibujito feo

            // Forzar el dibujado de todo y cambiar el buffer de ser necesario
            gl.wglSwapBuffers(csgl12Control.GetHDC());
        }
    }
}
