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
        public float xrot = 0.0f;				// X-axis rotation
        public float yrot = 0.0f;				// Y-axis rotation
        public float zrot = 0.0f;
        public float z = -5.0f;					// Depth Into The Screen

        public float xspeed = 0.1f;
        public float yspeed = 0.1f;

        // Lighting components for the cube
        public float[] LightAmbient = { 0.2f, 0.2f, 0.2f, 1.0f };
        public float[] LightDiffuse = { 0.2f, 0.2f, 0.2f, 1.0f };
        public float[] LightPosition = { 0.0f, 0.0f, 2.0f, 1.0f };

        public int filter = 0;					// Which Filter To Use
        public int[] texture = new int[6];	// Texture array

        Form f;

        public Handler()
        {

        }

        public void getParent(Form f)
        {
            this.f = f;
        }



        public void OpenGLStarted(CSGL12Control csgl12Control)
        {
            GL gl = csgl12Control.GetGL();

            if (null == gl) { return; }

            // Activar iluminación
            gl.glLightfv(GL.GL_LIGHT1, GL.GL_AMBIENT, this.LightAmbient);	// Setup The Ambient Light
            gl.glLightfv(GL.GL_LIGHT1, GL.GL_DIFFUSE, this.LightDiffuse);	// Setup The Diffuse Light
            gl.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, this.LightPosition);	// Position The Light
            gl.glEnable(GL.GL_LIGHT1);										// Enable Light One
            gl.glEnable(GL.GL_LIGHTING); //encender luz solo si está activada

            gl.glEnable(GL.GL_NORMALIZE); //garantizar vectores de luz apropiados

            LoadTextures(gl);
            gl.glEnable(GL.GL_TEXTURE_2D);									// Enable Texture Mapping
            gl.glShadeModel(GL.GL_SMOOTH);								// enable smooth shading

            gl.glClearColor(0.5f, 0.5f, 0.5f, 1.0f);					// black background
            gl.glClearDepth(1.0f);										// depth buffer setup
            gl.glEnable(GL.GL_DEPTH_TEST);								// enables depth testing
            gl.glDepthFunc(GL.GL_LEQUAL);								// type of depth testing
            gl.glHint(GL.GL_PERSPECTIVE_CORRECTION_HINT, GL.GL_NICEST);	// nice perspective calculations

            gl.glColor4f(1.0f, 1.0f, 1.0f, 0.8f);							// Full Brightness.  50% Alpha
            gl.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE);						// Set The Blending Function For Translucency

            // evitar parpadeos
            if (true == gl.bwglSwapIntervalEXT)
            {
                gl.wglSwapIntervalEXT(1);
            }
        }

        //Crear texturas
        protected bool LoadTextures(GL gl)
        {
            Bitmap image = null;
            string[] files = new string[6];
            files[0] = @"C:\Users\Victor\Documents\Workspace\Graficas\ElPlanetero2\ExamenFinal\Texturas\bot.png";
            files[1] = @"C:\Users\Victor\Documents\Workspace\Graficas\ElPlanetero2\ExamenFinal\Texturas\lado1.png";
            files[2] = @"C:\Users\Victor\Documents\Workspace\Graficas\ElPlanetero2\ExamenFinal\Texturas\lado2.png";
            files[3] = @"C:\Users\Victor\Documents\Workspace\Graficas\ElPlanetero2\ExamenFinal\Texturas\lado3.png";
            files[4] = @"C:\Users\Victor\Documents\Workspace\Graficas\ElPlanetero2\ExamenFinal\Texturas\lado5.png";
            files[5] = @"C:\Users\Victor\Documents\Workspace\Graficas\ElPlanetero2\ExamenFinal\Texturas\top.png";

            int idx = 0;
            gl.glGenTextures(6, this.texture);
            while (idx < files.Length)
            {
                try
                {
                    image = new Bitmap(files[idx]);
                }
                catch (System.ArgumentException)
                {
                    MessageBox.Show("Could not load " + files[idx] + ".  Please make sure that Data is a subfolder from where the application is running.", "Error", MessageBoxButtons.OK);
                    f.Dispose(); //Cerrar la forma
                }
                if (image != null)
                {
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    System.Drawing.Imaging.BitmapData bitmapdata;
                    Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                    bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    gl.glBindTexture(GL.GL_TEXTURE_2D, this.texture[idx]);
                    gl.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_NEAREST);
                    gl.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_NEAREST);
                    gl.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB, 513, 513, 0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

                    image.UnlockBits(bitmapdata);
                    image.Dispose();
                }
                idx = idx + 1;
            }
            return true;
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

            //Asignar un viewport  del tamaño del control
            gl.glViewport(0, 0, clientWidth, clientHeight);

            //Limpiar la pantalla con un color de fondo
            gl.glClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            gl.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            //Asignar la vista del modelo y la proyeccion
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

            //Aquí va tu dibujito chido

            gl.glEnable(GL.GL_LIGHTING);
            gl.glEnable(GL.GL_LIGHT2);
            gl.glDisable(GL.GL_LIGHT3);
            gl.glDisable(GL.GL_BLEND);			// Turn Blending Off
            gl.glEnable(GL.GL_DEPTH_TEST);		// Turn Depth Testing On
        
            gl.glTranslatef(0.0f, 0.0f, this.z);

            gl.glRotatef(xrot, 1.0f, 0.0f, 1.0f);
            gl.glRotatef(yrot, 0.0f, 1.0f, 1.0f);

            gl.glPushMatrix();
                gl.glScaled(10,10,10);

                gl.glBindTexture(GL.GL_TEXTURE_2D, this.texture[0]);
                gl.glBegin(GL.GL_QUADS);
                // Front Face
                gl.glNormal3f(0.0f, 0.0f, 1.0f);
                gl.glTexCoord2f(0.0f, 0.0f); gl.glVertex3f(-1.0f, -1.0f, 1.0f);
                gl.glTexCoord2f(1.0f, 0.0f); gl.glVertex3f(1.0f, -1.0f, 1.0f);
                gl.glTexCoord2f(1.0f, 1.0f); gl.glVertex3f(1.0f, 1.0f, 1.0f);
                gl.glTexCoord2f(0.0f, 1.0f); gl.glVertex3f(-1.0f, 1.0f, 1.0f);
                gl.glEnd();

                gl.glBindTexture(GL.GL_TEXTURE_2D, this.texture[1]);
                gl.glBegin(GL.GL_QUADS);
                // Back Face
                gl.glNormal3f(0.0f, 0.0f, -1.0f);
                gl.glTexCoord2f(1.0f, 0.0f); gl.glVertex3f(-1.0f, -1.0f, -1.0f);
                gl.glTexCoord2f(1.0f, 1.0f); gl.glVertex3f(-1.0f, 1.0f, -1.0f);
                gl.glTexCoord2f(0.0f, 1.0f); gl.glVertex3f(1.0f, 1.0f, -1.0f);
                gl.glTexCoord2f(0.0f, 0.0f); gl.glVertex3f(1.0f, -1.0f, -1.0f);
                gl.glEnd();

                gl.glBindTexture(GL.GL_TEXTURE_2D, this.texture[2]);
                gl.glBegin(GL.GL_QUADS);
                // Top Face
                gl.glNormal3f(0.0f, 1.0f, 0.0f);
                gl.glTexCoord2f(0.0f, 1.0f); gl.glVertex3f(-1.0f, 1.0f, -1.0f);
                gl.glTexCoord2f(0.0f, 0.0f); gl.glVertex3f(-1.0f, 1.0f, 1.0f);
                gl.glTexCoord2f(1.0f, 0.0f); gl.glVertex3f(1.0f, 1.0f, 1.0f);
                gl.glTexCoord2f(1.0f, 1.0f); gl.glVertex3f(1.0f, 1.0f, -1.0f);
                gl.glEnd();

                gl.glBindTexture(GL.GL_TEXTURE_2D, this.texture[3]);
                gl.glBegin(GL.GL_QUADS);
                // Bottom Face
                gl.glNormal3f(0.0f, -1.0f, 0.0f);
                gl.glTexCoord2f(1.0f, 1.0f); gl.glVertex3f(-1.0f, -1.0f, -1.0f);
                gl.glTexCoord2f(0.0f, 1.0f); gl.glVertex3f(1.0f, -1.0f, -1.0f);
                gl.glTexCoord2f(0.0f, 0.0f); gl.glVertex3f(1.0f, -1.0f, 1.0f);
                gl.glTexCoord2f(1.0f, 0.0f); gl.glVertex3f(-1.0f, -1.0f, 1.0f);
                gl.glEnd();

                gl.glBindTexture(GL.GL_TEXTURE_2D, this.texture[4]);
                gl.glBegin(GL.GL_QUADS);
                // Right face
                gl.glNormal3f(1.0f, 0.0f, 0.0f);
                gl.glTexCoord2f(1.0f, 0.0f); gl.glVertex3f(1.0f, -1.0f, -1.0f);
                gl.glTexCoord2f(1.0f, 1.0f); gl.glVertex3f(1.0f, 1.0f, -1.0f);
                gl.glTexCoord2f(0.0f, 1.0f); gl.glVertex3f(1.0f, 1.0f, 1.0f);
                gl.glTexCoord2f(0.0f, 0.0f); gl.glVertex3f(1.0f, -1.0f, 1.0f);
                gl.glEnd();

                gl.glBindTexture(GL.GL_TEXTURE_2D, this.texture[5]);
                gl.glBegin(GL.GL_QUADS);
                // Left Face
                gl.glNormal3f(-1.0f, 0.0f, 0.0f);
                gl.glTexCoord2f(0.0f, 0.0f); gl.glVertex3f(-1.0f, -1.0f, -1.0f);
                gl.glTexCoord2f(1.0f, 0.0f); gl.glVertex3f(-1.0f, -1.0f, 1.0f);
                gl.glTexCoord2f(1.0f, 1.0f); gl.glVertex3f(-1.0f, 1.0f, 1.0f);
                gl.glTexCoord2f(0.0f, 1.0f); gl.glVertex3f(-1.0f, 1.0f, -1.0f);
                gl.glEnd();

            gl.glPopMatrix();


            float[] pos = { 0.0f, 0.0f, 0.0f, 1.0f };
            float[] amb = { 1f, 1f, 1f, 1.0f };

            gl.glLightfv(GL.GL_LIGHT2, GL.GL_AMBIENT, amb);	// Setup The Ambient Light
            gl.glLightfv(GL.GL_LIGHT2, GL.GL_DIFFUSE, this.LightDiffuse);	// Setup The Diffuse Light
            gl.glLightfv(GL.GL_LIGHT2, GL.GL_POSITION, pos);	// Position The Light

            // Forzar el dibujado de todo y cambiar el buffer de ser necesario
            gl.wglSwapBuffers(csgl12Control.GetHDC());
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                xrot = xrot + xspeed;
            }
            else if (e.KeyCode == Keys.A)
            {
                xrot = xrot - xspeed;
            }
            else if (e.KeyCode == Keys.W)
            {
                yrot = yrot + yspeed;
            }
            else if (e.KeyCode == Keys.S)
            {
                yrot = yrot - yspeed;
            }
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {

        }

    }
}
