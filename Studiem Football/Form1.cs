using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using System.Threading;
using Tao.FreeGlut;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TAOSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            Gl.glClearColor(0, 0, 0, 0);
            Gl.glClearDepth(1.0);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, simpleOpenGlControl1.Width / (double)simpleOpenGlControl1.Height, 0.1, 100000);
            Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Glut.glutInit();
            grass = LoadTexture("grass.jpg");
            streetTexture = LoadTexture("Running.jpg");
            skybox = LoadTexture("skybox.jpg");
            skyboxdark = LoadTexture("skyboxDark3.jpg");
            LighTex = LoadTexture("lights.jpg");
            LighScoreTex =  LoadTexture("lightScore.jpg");
            advTexture = LoadTexture("adv.jpg");
            FansTex = LoadTexture("FansTex.jpg");
            flagsTexture = LoadTexture("flag.jpg");
            RunningStartTex = LoadTexture("RunningStart.jpg"); 
            
            try
            {
                //  treeTexture = LoadTextureWithMask("Tree.jpg", "TreeMask.jpg");
                //treeTexture = LoadTexture("Tree.jpg")
                //Gl.glNewList(1, Gl.GL_COMPILE);
                //drawHeightMap("heightmap.jpg", 200, 200);

               // g.Import3DS("Footballer.jpg");
                //g.Import3DS("1ball.3ds");
               // g.Import3DS("Ball1.3ds");
                g.Import3DS("SUPER8_L.3ds");
                
                //g.Import3DS("grenade.3DS");
            }
            catch (Exception)
            {
                Console.WriteLine("connut ");
            }
            
            //
            
            
            
            //Gl.glEndList();
        }

        int streetTexture, RunningStartTex, skybox, skyboxdark, grass, LighTex, LighScoreTex, advTexture, flagsTexture, FansTex;
            //studiumTexture, treeTexture,
        Mode3d g = new Mode3d();



        void drawSkybox()
        {
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, skybox);

            Gl.glBegin(Gl.GL_QUADS);


            //front
            Gl.glTexCoord2d(0.25, 0.33);
            Gl.glVertex3d(-1, -1, -1);
            Gl.glTexCoord2d(0.25, 0.66);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glTexCoord2d(0.5, 0.66);
            Gl.glVertex3d(1, 1, -1);
            Gl.glTexCoord2d(0.5, 0.33);
            Gl.glVertex3d(1, -1, -1);

            //right
            Gl.glTexCoord2d(0.5, 0.33);
            Gl.glVertex3d(1, -1, -1);
            Gl.glTexCoord2d(0.5, 0.66);
            Gl.glVertex3d(1, 1, -1);
            Gl.glTexCoord2d(0.75, 0.66);
            Gl.glVertex3d(1, 1, 1);
            Gl.glTexCoord2d(0.75, 0.33);
            Gl.glVertex3d(1, -1, 1);

            //back
            Gl.glTexCoord2d(0.75, 0.33);
            Gl.glVertex3d(1, -1, 1);
            Gl.glTexCoord2d(0.75, 0.66);
            Gl.glVertex3d(1, 1, 1);
            Gl.glTexCoord2d(1, 0.66);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glTexCoord2d(1, 0.33);
            Gl.glVertex3d(-1, -1, 1);

            //left
            Gl.glTexCoord2d(0, 0.33);
            Gl.glVertex3d(-1, -1, 1);
            Gl.glTexCoord2d(0, 0.66);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glTexCoord2d(0.25, 0.66);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glTexCoord2d(0.25, 0.33);
            Gl.glVertex3d(-1, -1, -1);

            //up
            Gl.glTexCoord2d(0.25, 0.66);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glTexCoord2d(0.25, 1);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glTexCoord2d(0.5, 1);
            Gl.glVertex3d(1, 1, 1);
            Gl.glTexCoord2d(0.5, 0.66);
            Gl.glVertex3d(1, 1, -1);

            //down
            Gl.glTexCoord2d(0.25, 0);
            Gl.glVertex3d(-1, -1, 1);
            Gl.glTexCoord2d(0.25, 0.33);
            Gl.glVertex3d(1, -1, 1);
            Gl.glTexCoord2d(0.5, 0.33);
            Gl.glVertex3d(1, -1, -1);
            Gl.glTexCoord2d(0.5, 0);
            Gl.glVertex3d(-1, -1, -1);
            Gl.glEnd();

            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
        }

        void drawSkyboxDark()
        {
            
            Gl.glDisable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, skyboxdark);

          
            Gl.glBegin(Gl.GL_QUADS);
           

            //front
            Gl.glTexCoord2d(0.25, 0.33);
            Gl.glVertex3d(-1, -1, -1);
            Gl.glTexCoord2d(0.25, 0.66);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glTexCoord2d(0.5, 0.66);
            Gl.glVertex3d(1, 1, -1);
            Gl.glTexCoord2d(0.5, 0.33);
            Gl.glVertex3d(1, -1, -1);

            //right
            Gl.glTexCoord2d(0.5, 0.33);
            Gl.glVertex3d(1, -1, -1);
            Gl.glTexCoord2d(0.5, 0.66);
            Gl.glVertex3d(1, 1, -1);
            Gl.glTexCoord2d(0.75, 0.66);
            Gl.glVertex3d(1, 1, 1);
            Gl.glTexCoord2d(0.75, 0.33);
            Gl.glVertex3d(1, -1, 1);

            //back
            Gl.glTexCoord2d(0.75, 0.33);
            Gl.glVertex3d(1, -1, 1);
            Gl.glTexCoord2d(0.75, 0.66);
            Gl.glVertex3d(1, 1, 1);
            Gl.glTexCoord2d(1, 0.66);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glTexCoord2d(1, 0.33);
            Gl.glVertex3d(-1, -1, 1);

            //left
            Gl.glTexCoord2d(0, 0.33);
            Gl.glVertex3d(-1, -1, 1);
            Gl.glTexCoord2d(0, 0.66);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glTexCoord2d(0.25, 0.66);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glTexCoord2d(0.25, 0.33);
            Gl.glVertex3d(-1, -1, -1);

            //up
            Gl.glTexCoord2d(0.25, 0.66);
            Gl.glVertex3d(-1, 1, -1);
            Gl.glTexCoord2d(0.25, 1);
            Gl.glVertex3d(-1, 1, 1);
            Gl.glTexCoord2d(0.5, 1);
            Gl.glVertex3d(1, 1, 1);
            Gl.glTexCoord2d(0.5, 0.66);
            Gl.glVertex3d(1, 1, -1);

            //down
            Gl.glTexCoord2d(0.25, 0);
            Gl.glVertex3d(-1, -1, 1);
            Gl.glTexCoord2d(0.25, 0.33);
            Gl.glVertex3d(1, -1, 1);
            Gl.glTexCoord2d(0.5, 0.33);
            Gl.glVertex3d(1, -1, -1);
            Gl.glTexCoord2d(0.5, 0);
            Gl.glVertex3d(-1, -1, -1);
            Gl.glEnd();

            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
        }

        double fx, fy, fz;
        double t =-80 , x = 30, z = 30, w, y = 15, tspeed = 0.1;
        Boolean isLighting, isDisKeys = false, isKeys = false; //, isRoundCam = false, isCam1 = false, isCam2 = false , isCam0= false; 
  //--------------------------------((PAINT))

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            fx = 30; fy = 15; fz = 30;
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);
            //----------------------CAMERA-------------------
            if (keydown.Contains(Keys.D0))
                camRound();
            if (keydown.Contains(Keys.D1))
                cam1();
            if (keydown.Contains(Keys.D2))
                cam2();
            else
               //isDisKeys = false;
                theCamera();
                Glu.gluLookAt(x, y, z, x + Math.Cos(t), y + Math.Sin(w), z + Math.Sin(t), 0, 1, 0);
                //-----------------------------
                Gl.glPushMatrix();
                Gl.glTranslated(x, y, z);
                if (isLighting)
                { Gl.glColor3d(.5, .2, .2); drawSkyboxDark(); }
                else
                    drawSkybox();
                Gl.glPopMatrix();
            
            //------------------------------------

            Gl.glPushMatrix();
            //g.Draw_3DS_Object(-10, 10, 30,10);
            g.Draw_3DS_Object( 30 , 7, -20, 0.001);  ///cam 1
            Gl.glPopMatrix();
            //-----------------------------------

            Gl.glPushMatrix();
            //---------------------------Bourder-------
            Gl.glPushMatrix();
                Gl.glTranslated(-50, 0, 150);
                Gl.glScaled(10, 1, 20);
                Gl.glPushMatrix();
                    Gl.glColor3d(.9, .9, .9);
                    drawBoardersDownTextured();
                    Gl.glPushMatrix();///---outer
                        Gl.glScaled(1.5, 3, 1.2);
                        Gl.glTranslated(-2.5, 0, 0);
                        Gl.glColor3d(0, .3, .6);
                        drawBoardersOuterTextured();// drawBoarders();
                    Gl.glPopMatrix();
                Gl.glPopMatrix();
            Gl.glPopMatrix();
            //--------------------------Colomns ---
            Gl.glPushMatrix();
            drawColomns();
            Gl.glPopMatrix();
            //-----------------Grass-------------------
            Gl.glPushMatrix();
            drawGrass();
            Gl.glPopMatrix();
            ////---------------------------RuningLine
            Gl.glPushMatrix();
            Gl.glTranslated(0, .75, 0);
            drawRuningLine();
            Gl.glPopMatrix();

            ////---------------------------drawStudiumLines
            Gl.glPushMatrix();
            Gl.glTranslated(0, -.25, 0);
            drawStudiumLines();
            Gl.glPopMatrix();
            ////---------------------------drawStudiumGoals
            Gl.glPushMatrix();
            Gl.glScaled(-.5 , 1 ,- .5);
            drawStudiumGoals();
            Gl.glPopMatrix();
            //------------------------------------
            Gl.glPopMatrix();

        }
 //-----------------((Methods ))-------------------     
        private void drawLightingENABLE0(float x , float y , float z )
        {
            Gl.glEnable(Gl.GL_LIGHT0);

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, 60);

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { x, y, y, 1 });

        }
   
        private void drawLightingENABLE1(float x, float y, float z)
        {
            Gl.glEnable(Gl.GL_LIGHT1);

            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, 60);

            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, new float[] { x, y, y, 0 });

        }

        private void drawLightingENABLE2(float x, float y, float z)
        {
            Gl.glEnable(Gl.GL_LIGHT2);

            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_DIFFUSE, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, 60);

            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, new float[] { x, y, y, 0 });

        }

        private void drawLightingENABLE3(float x, float y, float z)
        {
            Gl.glEnable(Gl.GL_LIGHT3);

            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_DIFFUSE, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, 60);

            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_POSITION, new float[] { x, y, y, 0 });

        }

        private void drawLightingENABLE4(float x, float y, float z)
        {
            Gl.glEnable(Gl.GL_LIGHT4);

            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_DIFFUSE, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, 60);

            Gl.glLightfv(Gl.GL_LIGHT4, Gl.GL_POSITION, new float[] { x, y, y, 0 });

        }

        private void drawLightingTextured(double x1, double y1,double z1, double x2, double y2 , double z2)
        {
            axieY = 0;
            axieY2 = 0;
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, LighTex);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL); //modolte

            Gl.glBegin(Gl.GL_QUADS);

            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(x1, axieY, y1);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(x1, axieY2, y2);
            Gl.glTexCoord2d(1, 1);
            Gl.glVertex3d(x2, axieY2, y2);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(x2, axieY, y1);
            Gl.glEnd();

            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }
       
        private void drawScoreLightTextured(double x1, double y1,double z1, double x2, double y2 , double z2)
        {
            axieY = 0;
            axieY2 = 0;
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, LighScoreTex);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL); //modolte

            Gl.glBegin(Gl.GL_QUADS);

            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(x1, axieY, y1);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(x1, axieY, y2);
            Gl.glTexCoord2d(1, 1);
            Gl.glVertex3d(x2, axieY2, y2);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(x2, axieY2, y1);
            Gl.glEnd();

            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }
        
        private void drawColomns()
        {
            drawLightingENABLE0(-100, 100, 100);///////////////////////////////////LLLL
            Gl.glPushMatrix();
            Gl.glRotated(-90, 1, 0, 0);
            Gl.glColor3d(0.2, 0.2, 0.2);
            Gl.glPushMatrix();
            //////////((Score))
            Gl.glPushMatrix();
            Gl.glColor3d(0.02, 0.02, 0.02);
            Gl.glTranslated(-100, - 200, 0);
            Glut.glutSolidCone(5, 50, 30, 300);//
            Gl.glPushMatrix();
            Gl.glRotated(90, 0, 1, 0);
            Gl.glRotated(90, 0, 0, 1);
            Gl.glRotated(-90, 1, 0, 0);
            drawScoreLightTextured(-25, 30, 0,25, 70, 0);
            Gl.glTranslated(0, -2, 0);
            drawScoreLightTextured(-25, 30, 0, 25, 70, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
//------------------------ELES
            // Glu.gluQuadricTexture(Glu.gluNewQuadric(), LighTex);
            Gl.glPushMatrix();
            Gl.glTranslated(-75, 100, 3);
            Gl.glPushMatrix();
            drawLightingENABLE1(1, -1, 1);///////////////////////////////////LLLL
            drawLightingTextured(-15, 45,0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //-----------------------------
            Gl.glPushMatrix();
            Gl.glTranslated(-75, 100 , 0);
            Glut.glutSolidCone(2.5, 50, 30, 300);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 12.5, 0);
            Gl.glRotated(30, 1, 0, 1);
            drawLightingTextured(-15, 45, 0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //---------++++++++++++++++++++++++++++++++++++++2--
            Gl.glPushMatrix();
            Gl.glTranslated(75, 100, 0);
            drawLightingENABLE2(-1, -1, 1);///////////////////////////////////LLLL2
            Glut.glutSolidCone(2.5, 50, 30, 300);
            Gl.glPushMatrix(); 
            drawLightingTextured(-15, 45, 0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //-----------------------------
            Gl.glPushMatrix();
            Gl.glTranslated(75, 100, 3);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 12.5, 0);
            Gl.glRotated(30, 1,0, 1);//////////////////////
            drawLightingTextured(-15, 45, 0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //--------------+++++++++++++++++++++++++++++++++3--------
            Gl.glPushMatrix();
            Gl.glTranslated(-75, -500, 0);
            drawLightingENABLE3(1, -1, -1);///////////////////////////////////LLLL3
            Glut.glutSolidCone(2.5, 50, 30, 300);
            Gl.glPushMatrix();
            Gl.glTranslated(0, -12.5, 0);
            Gl.glRotated(-30, 1, 0, 1);////////////////////
            drawLightingTextured(-15, 45, 0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //-----------------------------
            Gl.glPushMatrix();
            Gl.glTranslated(-75, -500, 0);
            Gl.glPushMatrix();
            drawLightingTextured(-15, 45, 0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //---------+++++++++++++++++++++++++++++++++++++++++++4--
            Gl.glPushMatrix();
            Gl.glTranslated(75, -500, 0);
            Gl.glPushMatrix();
            Gl.glTranslated(0,-12.5, 0);
            Gl.glRotated(-30, 1, 0, 1);///////////////////
            drawLightingTextured(-15, 45, 0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //-----------------------------
            Gl.glPushMatrix();
            Gl.glTranslated(75, -500, 0);
            drawLightingENABLE4(-1, -1, -1);///////////////////////////////////LLLL4
            Glut.glutSolidCone(2.5, 50, 30, 300);
            Gl.glPushMatrix();
            drawLightingTextured(-15, 45, 0, 15, 60, 0);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //---------++++++++++++++++++++++++++--
            Gl.glPopMatrix();

            
        }
        
        private void drawBoardersDownTextured ( ) //(double x1, double y1, double x2, double y2)
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, advTexture);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);

            //--------- -10 , 0 , -20  /////  10 , 10 , 20
            ////////////////////////////////////////////////////////////////////////////////////////////
            Gl.glBegin(Gl.GL_QUADS);
           //////////
            //  Gl.glColor3d(1, 1, 1);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 0, 0); ///// right to left side
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(-10, 0, 20);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(-10, 10, 20);
            Gl.glTexCoord2d(1, 1);
            Gl.glVertex3d(-10, 10, -20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(-10, 0, -20);
            // Gl.glEnd();
            ///////////////////////////
            //Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(0, 0, -1); /////// front to back
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(-10, 0, 20); //
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(-10, 10, 20);
            Gl.glTexCoord2d(1,1);
            Gl.glVertex3d(20, 10, 20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(20, 0, 20);
            //Gl.glEnd();
            //////////////////
            //Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, 0, 0); //// left to right side 
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(20, 0, 20);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(20, 10, 20);
            Gl.glTexCoord2d(1,1);
            Gl.glVertex3d(20, 10, -20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(20, 0, -20);
            //Gl.glEnd();
            ////////////////////
            // Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(0, 0, 1); //// back to front
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(20, 0, -20);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(20, 10, -20);
            Gl.glTexCoord2d(1, 1);
            Gl.glVertex3d(-10, 10, -20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(-10, 0, -20);
           // Gl.glEnd();
            ///////////////
            Gl.glEnd();
            Gl.glDisable(Gl.GL_TEXTURE_2D);

        }
        
         private void drawBoardersOuterTextured ( ) //(double x1, double y1, double x2, double y2)
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, FansTex);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);

            //--------- -10 , 0 , -20  /////  10 , 10 , 20
            ////////////////////////////////////////////////////////////////////////////////////////////
            Gl.glBegin(Gl.GL_QUADS);
           //////////
            //  Gl.glColor3d(1, 1, 1);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 0, 0); ///// right to left side
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(-10, 0, 20);
            Gl.glTexCoord2d(0, .5);
            Gl.glVertex3d(-10, 10, 20);
            Gl.glTexCoord2d(1, .5);
            Gl.glVertex3d(-10, 10, -20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(-10, 0, -20);
            // Gl.glEnd();
            ///////////////////////////
            //Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(0, 0, -1); /////// front to back
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(-10, 0, 20); //
            Gl.glTexCoord2d(0, .5);
            Gl.glVertex3d(-10, 10, 20);
            Gl.glTexCoord2d(1,.5);
            Gl.glVertex3d(20, 10, 20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(20, 0, 20);
            //Gl.glEnd();
            //////////////////
            //Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, 0, 0); //// left to right side 
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(20, 0, 20);
            Gl.glTexCoord2d(0, .5);
            Gl.glVertex3d(20, 10, 20);
            Gl.glTexCoord2d(1,.5);
            Gl.glVertex3d(20, 10, -20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(20, 0, -20);
            //Gl.glEnd();
            ////////////////////
            // Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(0, 0, 1); //// back to front
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(20, 0, -20);
            Gl.glTexCoord2d(0, .5);
            Gl.glVertex3d(20, 10, -20);
            Gl.glTexCoord2d(1, .5);
            Gl.glVertex3d(-10, 10, -20);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(-10, 0, -20);
           // Gl.glEnd();
            ///////////////
            Gl.glEnd();
            Gl.glDisable(Gl.GL_TEXTURE_2D);

        }
        
        private void drawStudiumGoals()
        {
          Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -800);
            Gl.glPushMatrix();
                Gl.glColor3d(0.9, 0.9, 0.9);
                Gl.glTranslated(-25, 0, 0);
                Gl.glRotated(-90, 1, 0, 0);
                Glut.glutSolidCylinder(1, 10, 30, 300);
                // Gl.glTranslated(0, -400, 0);
                Gl.glTranslated(0, -800, 0);
                Glut.glutSolidCylinder(1, 10, 30, 300);
                //Gl.glTranslated(0, 400, 0); ///////up 
                Gl.glTranslated(0, 800, 0);
                Gl.glRotated(90, 0, 1, 0);
                Gl.glTranslated(-10, 0, 0);
                Glut.glutSolidCylinder(1, 51, 30, 300);
           Gl.glPopMatrix();

           Gl.glPushMatrix();
                Gl.glColor3d(0.9, 0.9, 0.9);
                Gl.glTranslated(25, 0, 0);
                Gl.glRotated(-90, 1, 0, 0);
                Glut.glutSolidCylinder(1, 10, 300, 300);
                //Gl.glTranslated(0, -400, 0);
                Gl.glTranslated(0, -800, 0); ////// otherSide
                Glut.glutSolidCylinder(1, 10, 300, 300);
                //Gl.glTranslated(0, 400, 0);
                Gl.glRotated(90, 0, 1, 0);/////// up 
                Gl.glTranslated(-10, 0, 0);
                Gl.glRotated(180, 1, 0, 0);
                Glut.glutSolidCylinder(1, 51, 30, 300);
           Gl.glPopMatrix();
       Gl.glPopMatrix();
//-----------------------------#$%#$%@#$-----------------Mesh
       Gl.glPushMatrix();

       Gl.glTranslated(0, 0, -800);
                Gl.glPushMatrix();
                    Gl.glTranslated(-25, 0, -8);
                    Gl.glPushMatrix();
                    Gl.glRotated(-90, 0, 1, 0);
                    Gl.glColor3d(0.9, 0.9, 0.9);
                    for (double i = 0; i < 8; i += 0.1)//coll
                    {
                        for (double w = 0; w < 10; w += 0.2) // row
                        {
                            Gl.glBegin(Gl.GL_TRIANGLES);
                            Gl.glVertex3d(-8, w, 0);
                            Gl.glVertex3d(-8, w, 0);
                            Gl.glVertex3d(-8, w + 0.01, 0);
                            Gl.glEnd();

                            Gl.glBegin(Gl.GL_TRIANGLES);
                            Gl.glVertex3d(i, 0, 0);
                            Gl.glVertex3d(i + 0.01, 0, 0);
                            Gl.glVertex3d(i, w, 0);
                            Gl.glEnd();
                        }
                    }
                    Gl.glTranslated(0, 0, -50);
                    for (double w = 0; w < 10; w += 0.2) // row
                    {
                        Gl.glBegin(Gl.GL_TRIANGLES);
                        Gl.glVertex3d(-8, w, 0);
                        Gl.glVertex3d(-8, w, 0);
                        Gl.glVertex3d(-8, w + 0.01, 0);
                        Gl.glEnd();
                    }
                    for (double i = 0; i < 8; i += 0.1)//coll
                    {
                      

                            Gl.glBegin(Gl.GL_TRIANGLES);
                            Gl.glVertex3d(i, 0, 0);
                            Gl.glVertex3d(i + 0.01, 0, 0);
                            Gl.glVertex3d(i, 10, 0);
                            Gl.glEnd();
                        
                    }
                      Gl.glPopMatrix();
                //---------mayeel
                      Gl.glRotated(45, 1, 0, 0);
                      Gl.glTranslated(0, -5, -3);
                      Gl.glColor3d(0.9, 0.9, 0.9);
                      for (double i = 0; i < 50; i += 0.1)//coll
                      {
                          Gl.glBegin(Gl.GL_QUADS);
                          Gl.glVertex3d(i, 0, 0);
                          Gl.glVertex3d(i + 0.01, 0, 0);
                          Gl.glVertex3d(i + 0.01, 2, 0);
                          Gl.glVertex3d(i, 2, 0);
                          Gl.glEnd();
                      }
                      for (double w = 0; w < 17; w += 0.2) // row
                      {
                          Gl.glBegin(Gl.GL_QUADS);
                          Gl.glVertex3d(0, w, 0);
                          Gl.glVertex3d(50, w, 0);
                          Gl.glVertex3d(50, w + 0.01, 0);
                          Gl.glVertex3d(0, w + 0.01, 0);
                          Gl.glEnd();
                      }
    Gl.glPopMatrix();
 Gl.glPopMatrix();

    //----------------###--------------new-----------Mesh
    Gl.glPushMatrix();
    Gl.glRotated(180, 0, 1, 0);
        Gl.glPushMatrix();
                Gl.glTranslated(-25, 0, -8);
                Gl.glPushMatrix();
                    Gl.glRotated(-90, 0, 1, 0);
                    Gl.glColor3d(0.9, 0.9, 0.9);
                    for (double i = 0; i < 8; i += 0.1)//coll
                    {
                        for (double w = 0; w < 10; w += 0.2) // row
                        {
                            Gl.glBegin(Gl.GL_TRIANGLES);
                            Gl.glVertex3d(-8, w, 0);
                            Gl.glVertex3d(-8, w, 0);
                            Gl.glVertex3d(-8, w + 0.01, 0);
                            Gl.glEnd();

                            Gl.glBegin(Gl.GL_TRIANGLES);
                            Gl.glVertex3d(i, 0, 0);
                            Gl.glVertex3d(i + 0.01, 0, 0);
                            Gl.glVertex3d(i, w, 0);
                            Gl.glEnd();
                        }
                    }
                    Gl.glTranslated(0, 0, -50);
                    for (double i = 0; i < 8; i += 0.1)//coll
                    {
                        for (double w = 0; w < 10; w += 0.2) // row
                        {
                            Gl.glBegin(Gl.GL_TRIANGLES);
                            Gl.glVertex3d(-8, w, 0);
                            Gl.glVertex3d(-8, w, 0);
                            Gl.glVertex3d(-8, w + 0.01, 0);
                            Gl.glEnd();

                            Gl.glBegin(Gl.GL_TRIANGLES);
                            Gl.glVertex3d(i, 0, 0);
                            Gl.glVertex3d(i + 0.01, 0, 0);
                            Gl.glVertex3d(i, w, 0);
                            Gl.glEnd();
                        }
                    }
                Gl.glPopMatrix();
                //---------mayeel
                Gl.glRotated(45, 1, 0, 0);
                Gl.glTranslated(0, -5, -3);
                Gl.glColor3d(0.9, 0.9, 0.9);
                for (double i = 0; i < 50; i += 0.1)//coll
                {
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glVertex3d(i, 0, 0);
                    Gl.glVertex3d(i + 0.01, 0, 0);
                    Gl.glVertex3d(i + 0.01, 2, 0);
                    Gl.glVertex3d(i, 2, 0);
                    Gl.glEnd();
                }
                for (double w = 0; w < 17; w += 0.2) // row
                {
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glVertex3d(0, w, 0);
                    Gl.glVertex3d(50, w, 0);
                    Gl.glVertex3d(50, w + 0.01, 0);
                    Gl.glVertex3d(0, w + 0.01, 0);
                    Gl.glEnd();
                }
       Gl.glPopMatrix();
  Gl.glPopMatrix();
    //-----------$$$----------------------------------------

          

        }
   
        private void drawStudiumLines()
        {
           
                Gl.glPushMatrix();
                drawFlags();
                Gl.glPopMatrix();
            
            Gl.glColor3d(1, 1, 1);
            Gl.glPushMatrix();
            Gl.glColor3d(1, 1, 1);
            ///----------------------outer1
            Gl.glPushMatrix();
            Gl.glColor3d(1, 1, 1);
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex3d(-45, 0.25, 400);
            Gl.glVertex3d(45, 0.25, 400);
            Gl.glVertex3d(45, 0.25, 0);
            Gl.glVertex3d(-45, 0.25, 0);
            Gl.glEnd();
            Gl.glPopMatrix();

            //--------------middle
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex3d(-45, 0.25, 200);
            Gl.glVertex3d(45, 0.25, 200);
            Gl.glEnd();
            //--------------------------
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_LINE_STRIP); /// cir
            for (double d = 0; d <= Math.PI * 2; d += Math.PI / 30.0)
            {
                double x = Math.Cos(d) * 30;
                double z = Math.Sin(d) * 30;
                Gl.glVertex3d(x, .025, z + 200);
            }
            Gl.glEnd();
            Gl.glFlush();
            Gl.glPopMatrix();
            //-------------------------------point
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_POLYGON);
            for (double d = 0; d <= Math.PI * 2; d += Math.PI / 30.0)
            {
                double x = Math.Cos(d) * 2;
                double z = Math.Sin(d) * 2;
                Gl.glVertex3d(x, .25, z + 200);
            }
            Gl.glEnd();
            Gl.glFlush();
            Gl.glPopMatrix();
            //-------------------------ghaza2---1--
         
            Gl.glPushMatrix();
            Gl.glColor3d(1, 1, 1);


            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex3d(-40, 0.25, 400);
            Gl.glVertex3d(40, 0.25, 400);
            Gl.glVertex3d(40, 0.25, 300);
            Gl.glVertex3d(-40, 0.25, 300);
            Gl.glEnd();

            Gl.glColor3d(1, 1, 1);
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex3d(-25, 0.25, 400);
            Gl.glVertex3d(25, 0.25, 400);
            Gl.glVertex3d(25, 0.25, 375);
            Gl.glVertex3d(-25, 0.25, 375);
            Gl.glEnd();
            //-------------------------------point
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_POLYGON);
            for (double d = 0; d <= Math.PI * 2; d += Math.PI / 30.0)
            {
                double x = Math.Cos(d);
                double z = Math.Sin(d);
                Gl.glVertex3d(x, .25, z + 350);
            }
            Gl.glEnd();
            Gl.glFlush();
            Gl.glPopMatrix();

            ///------------------- cir
            Gl.glPushMatrix();
            Gl.glRotated(180, 1, 0, 0);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            for (double d = 0; d <= Math.PI * 1.01; d += Math.PI / 30.0)
            {
                double x = Math.Cos(d) * 10;
                double z = Math.Sin(d) * 10;
                Gl.glVertex3d(x, -.25, z - 300);
            }
            Gl.glEnd();
            Gl.glFlush();
            Gl.glPopMatrix();


            //-------------------------ghaza2---2--
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex3d(-40, 0.25, 100);
            Gl.glVertex3d(40, 0.25, 100);
            Gl.glVertex3d(40, 0.25, 0);
            Gl.glVertex3d(-40, 0.25, 0);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex3d(-25, 0.25, 35);
            Gl.glVertex3d(25, 0.25, 35);
            Gl.glVertex3d(25, 0.25, 0);
            Gl.glVertex3d(-25, 0.25, 0);
            Gl.glEnd();
            Gl.glPopMatrix();
            //-------------------------------point
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_POLYGON);
            for (double d = 0; d <= Math.PI * 2; d += Math.PI / 30.0)
            {
                double x = Math.Cos(d);
                double z = Math.Sin(d);
                Gl.glVertex3d(x, .25, z + 50);
            }
            Gl.glEnd();
            Gl.glFlush();
            Gl.glPopMatrix();

            ///----------- cir
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_LINE_STRIP);
            for (double d = 0; d <= Math.PI * 1.01; d += Math.PI / 30.0)
            {
                double x = Math.Cos(d) * 10;
                double z = Math.Sin(d) * 10;
                Gl.glVertex3d(x, .25, z + 100);
            }
            Gl.glEnd();
            Gl.glFlush();
            Gl.glPopMatrix();

            Gl.glPopMatrix();

        }

        private void drawFlags() 
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, flagsTexture);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);
            //-------------Flag------------- 45 , 400
            Gl.glPushMatrix();
            Gl.glTranslated(-45, 0, 0);
            Gl.glColor3d(1, 1, 1); //color
                Gl.glPushMatrix();
                    Gl.glScaled(.5, 1, .5);     
                    Gl.glRotated(-90, 1, 0, 0);
                    Glut.glutSolidCylinder(.5, 5, 10, 20);
                Gl.glPopMatrix();
                Gl.glPushMatrix();
                    Gl.glScaled(2, 2, 2);  
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2,  0 );
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2.5, 0 );
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5,  0);
                    Gl.glEnd();
                Gl.glPopMatrix();
                Gl.glPushMatrix(); /// --repeat
                    Gl.glScaled(2, 2, 2);
                    Gl.glRotated(-90, 0, 1, 0);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2, 0);
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2.5, 0);
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5, 0);
                    Gl.glEnd();
                Gl.glPopMatrix();

            Gl.glTranslated(90, 0, 0);//// othes2
                Gl.glPushMatrix();
                    Gl.glScaled(.5, 1, .5);
                    Gl.glRotated(-90, 1, 0, 0);
                    Glut.glutSolidCylinder(.5, 5, 10, 20);
                Gl.glPopMatrix();
                Gl.glPushMatrix();
                    Gl.glScaled(2, 2, 2);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2.5, 0);
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5, 0);
                    Gl.glEnd();
                Gl.glPopMatrix();
                Gl.glPushMatrix(); /// --repeat
                    Gl.glScaled(2, 2, 2);
                    Gl.glRotated(-90, 0, 1, 0);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2.5, 0);
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5, 0);
                    Gl.glEnd();
                Gl.glPopMatrix();

                Gl.glTranslated(0, 0, 400);//// othes3
                Gl.glPushMatrix();
                    Gl.glScaled(.5, 1, .5);
                    Gl.glRotated(-90, 1, 0, 0);
                    Glut.glutSolidCylinder(.5, 5, 10, 20);
                Gl.glPopMatrix();
                Gl.glPushMatrix();
                    Gl.glScaled(2, 2, 2);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2.5, 0);
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5, 0);
                    Gl.glEnd();
                Gl.glPopMatrix();
                Gl.glPushMatrix(); /// --repeat
                    Gl.glScaled(2, 2, 2);
                    Gl.glRotated(-90, 0, 1, 0);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2.5, 0);
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5, 0);
                    Gl.glEnd();
                Gl.glPopMatrix();

                Gl.glTranslated(-90, 0, 0);//// othes4
                Gl.glPushMatrix();
                    Gl.glScaled(.5, 1, .5);
                    Gl.glRotated(-90, 1, 0, 0);
                    Glut.glutSolidCylinder(.5, 5, 10, 20);
                    Gl.glPopMatrix();
                    Gl.glPushMatrix();
                    Gl.glScaled(2, 2, 2);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2.5, 0);
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5, 0);
                    Gl.glEnd();
                    Gl.glPopMatrix();
                Gl.glPushMatrix(); /// --repeat
                    Gl.glScaled(2, 2, 2);
                    Gl.glRotated(-90, 0, 1, 0);
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glTexCoord2d(0, 0);
                    Gl.glVertex3d(-1, 2, 0);
                    Gl.glTexCoord2d(0, 1);
                    Gl.glVertex3d(1, 2, 0);
                    Gl.glTexCoord2d(1, 1);
                    Gl.glVertex3d(1, 2.5, 0);
                    Gl.glTexCoord2d(1, 0);
                    Gl.glVertex3d(-1, 2.5, 0);
                    Gl.glEnd();
                Gl.glPopMatrix();
            Gl.glPopMatrix();
            //----------------------------------@#123--
            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }

        private void drawRuningLine()
        {
            Gl.glPushMatrix();
            drawStartRunning();
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            // Gl.glScaled(0.15, 0.15, 0.15);
            Gl.glPushMatrix();
            drawPartOfRoad(-30, 0, 0, 0, Math.PI / 2, 50);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            drawStraightStreet(-50, 400, -80, 0);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            // Gl.glScaled(0.15, 0.15, 0.15);
            Gl.glRotated(180, 1, 0, 0);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -400);
            drawPartOfRoad(-30, 0, 0, 0, Math.PI / 2, 50);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            //-------------------left part
            Gl.glPushMatrix();
            Gl.glRotated(180, 0, 0, 1);
            Gl.glPushMatrix();
            drawPartOfRoad(-30, 0, 0, 0, Math.PI / 2, 50);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            drawStraightStreet(-50, 400, -80, 0);
            Gl.glPopMatrix();
            Gl.glPushMatrix();
            // Gl.glScaled(0.15, 0.15, 0.15);
            Gl.glRotated(180, 1, 0, 0);
            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -400);
            drawPartOfRoad(-30, 0, 0, 0, Math.PI / 2, 50);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
            Gl.glPopMatrix();

        }

        private void  drawStartRunning()
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, RunningStartTex);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);

            
            Gl.glTranslated(80, 0, 90);
            //Gl.glScaled(1, 1, 1);
            Gl.glRotated(-90, 0, 1, 0);
            //Gl.glScaled(0.01, 1, 0.01);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(-30, 0, 0);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3d(-50, 0, 0);
            Gl.glTexCoord2d(1, 1);
            Gl.glVertex3d(-50, 0, 30);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(-30, 0, 30);
            Gl.glEnd();

            Gl.glDisable(Gl.GL_TEXTURE_2D);
        
        }

        private void drawGrass()
        {
            Gl.glPushMatrix();
            Gl.glColor3d(0, 1, 0);
            Gl.glRotated(90, 1, 0, 0);
            Gl.glTranslated(0, 0, .5);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, grass);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(-1000, -1000, 0);
            Gl.glTexCoord2d(0, 100);
            Gl.glVertex3d(-1000, 1000, 0);
            Gl.glTexCoord2d(100, 100);
            Gl.glVertex3d(1000, 1000, 0);
            Gl.glTexCoord2d(100, 0);
            Gl.glVertex3d(1000, -1000, 0);
            Gl.glEnd();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();

        }


        double axieY, axieY2;
        void drawStraightStreet(double x1, double y1, double x2, double y2)
        {
            axieY = 0;
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, streetTexture);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);

            Gl.glBegin(Gl.GL_QUADS);

            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3d(x1, axieY, y1);
            Gl.glTexCoord2d(0, 40);
            Gl.glVertex3d(x1, axieY, y2);
            Gl.glTexCoord2d(1, 40);
            Gl.glVertex3d(x2, axieY, y2);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3d(x2, axieY, y1);
            Gl.glEnd();

            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }

        void drawPartOfRoad(double x1, double y1, double x2, double y2, double angle, double length)
        {
            axieY = 0;
            double l = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            double centerX = (x2 - x1) * length / l;
            double centerY = (y2 - y1) * length / l;

            double startAngle = Math.Acos(x2 - centerX / length);

            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, streetTexture);

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);

            Gl.glBegin(Gl.GL_QUADS);

            for (double s = startAngle; s < startAngle + angle; s += 0.1)
            {
                double s1 = s + 0.1;
                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3d(Math.Cos(s) * length, axieY, Math.Sin(s) * length);
                Gl.glTexCoord2d(0, 1);
                Gl.glVertex3d(Math.Cos(s1) * length, axieY, Math.Sin(s1) * length);
                Gl.glTexCoord2d(1, 1);
                Gl.glVertex3d(Math.Cos(s1) * (length + l), axieY, Math.Sin(s1) * (length + l));
                Gl.glTexCoord2d(1, 0);
                Gl.glVertex3d(Math.Cos(s) * (length + l), axieY, Math.Sin(s) * (length + l));
            }

            Gl.glEnd();


            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }


        void theCamera()
        {
            //label1.Text = "X =" + x.ToString();
            //label2.Text = "Y =" + y.ToString() + "  w =" + w.ToString();
            //label3.Text = "Z =" + z.ToString();
            if (keydown.Contains(Keys.D0))
            { isKeys = true; isDisKeys = false; }
            if (isDisKeys) return ;
            if (keydown.Contains(Keys.L))
            {
                isLighting = true;
                Gl.glEnable(Gl.GL_LIGHTING);
            }
            if (keydown.Contains(Keys.K))
            {
                isLighting = false;
                Gl.glDisable(Gl.GL_LIGHTING);
            }
            if (keydown.Contains(Keys.Left))
                t -= tspeed;
            if (keydown.Contains(Keys.Right))
                t += tspeed;
            if (keydown.Contains(Keys.Up))
            {
                x += Math.Cos(t);
                z += Math.Sin(t);
                if (keydown.Contains(Keys.Space))
                {

                    x += Math.Cos(t) * 15;
                    z += Math.Sin(t) * 15;
                }
            }
            if (keydown.Contains(Keys.Down))
            {
                x -= Math.Cos(t);
                z -= Math.Sin(t);
                if (keydown.Contains(Keys.Space))
                {

                    x -= Math.Cos(t) * 15;
                    z -= Math.Sin(t) * 15;
                }
            }

            if (keydown.Contains(Keys.W))
                w += 0.1;
            if (keydown.Contains(Keys.S))
                w -= 0.1;
            if (keydown.Contains(Keys.OemMinus))
            {
                y -= tspeed;
                if (keydown.Contains(Keys.Space))
                {

                    y /= 1.2;

                }
            }
            if (keydown.Contains(Keys.Oemplus))
            {
                y += tspeed;
                if (keydown.Contains(Keys.Space))
                {

                    y *= 1.2;

                }
            }
            if (keydown.Contains(Keys.PageUp))
            {
                x += 15 * Math.Cos(t);
                z += 15 * Math.Sin(t);
            }
            if (keydown.Contains(Keys.PageDown))
            {
                x -= 15 * Math.Cos(t);
                z -= 15 * Math.Sin(t);
            }
            //if (keydown.Contains(Keys.D0))
            //    camRound();
            //if (keydown.Contains(Keys.D9))
            //    theCamera();
            //if (keydown.Contains(Keys.D1))
            //    cam1();
            //if (keydown.Contains(Keys.D2))
                //cam2();


        }

        void cam1()
        {
            //isDisKeys = true; 
            //isCam1 = true; 
            //--------------middle
          //  fx = 30; fy = 15; fz = 30;
            double x = fx - 35, y = fy +5 , z = fz  -20; 
            double xd = -5  , yd = 2 , zd = -200 ;
            Glu.gluLookAt(x, y, z, xd, yd, zd, 0, 1, 0.25);     
            Gl.glTranslated(x, y, z);
            if (isLighting)
            { Gl.glColor3d(.5, .2, .2); drawSkyboxDark(); }
            else
                drawSkybox();
            Gl.glTranslated(-1, 4, -30);

         
        }
        void cam2()
        {
              //isDisKeys = true; 
            //isCam1 = true; 
            //  fx = 30; fy = 15; fz = 30;
            //--------------middle
           // Gl.glTranslated(0, 0, 20);//// othes3
            Gl.glRotated(-180, 0, 1, 0);
            double x = fx + 2, y = fy - 10, z = fz + 2;
            double xd = -10, yd = 2, zd = -200;
            Glu.gluLookAt(x, y, z, xd, yd, zd, 0, 1, 0.25);
            Gl.glTranslated(x, y, z);
            if (isLighting)
            { Gl.glColor3d(.5, .2, .2); drawSkyboxDark(); }
            else
                drawSkybox();
            Gl.glTranslated(-1, 4, -70);
            Gl.glRotated(-180, 0, 1, 0);
        }

        double t1, t2, r = 5;
        void camRound() 
        {
            if (keydown.Contains(Keys.Left))
                t1 -= 0.1;
            if (keydown.Contains(Keys.Right))
                t1 += 0.1;
            if (keydown.Contains(Keys.Up))
            {
                t2 += 0.1;
            }
            if (keydown.Contains(Keys.Down))
            {
                t2 -= 0.1;
            }
            if (keydown.Contains(Keys.W))
                r += 1;
            if (keydown.Contains(Keys.S))
                r -= 1;
            double x = Math.Cos(t1) * Math.Cos(t2) * r;
            double y = Math.Sin(t2) * r;
            double z = Math.Sin(t1) * Math.Cos(t2) * r;
            Random rr = new Random();

            

            Glu.gluLookAt(x, y, z, 0, 0, 0, 0, 1, 0);
          

        }





        /*--------------------------------------*/
        private void drawBoarders()
        {

            ////////////////////////////////////////////////////////////////////////////////////////////
            //  Gl.glColor3d(1, 1, 1);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 0, 0); ///// right to left side
            Gl.glVertex3d(-10, 0, 20);
            Gl.glVertex3d(-10, 10, 20);
            Gl.glVertex3d(-10, 10, -20);
            Gl.glVertex3d(-10, 0, -20);

            // Gl.glEnd();

            //Gl.glBegin(Gl.GL_QUADS);

            Gl.glNormal3d(0, 0, -1); /////// front to back
            Gl.glVertex3d(-10, 0, 20); //
            Gl.glVertex3d(-10, 10, 20);
            Gl.glVertex3d(20, 10, 20);
            Gl.glVertex3d(20, 0, 20);

            //Gl.glEnd();
            //////////////////
            //Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, 0, 0); //// left to right side 
            Gl.glVertex3d(20, 0, 20);
            Gl.glVertex3d(20, 10, 20);
            Gl.glVertex3d(20, 10, -20);
            Gl.glVertex3d(20, 0, -20);
            //Gl.glEnd();
            ////////////////////
            // Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(0, 0, 1); //// back to front
            Gl.glVertex3d(20, 0, -20);
            Gl.glVertex3d(20, 10, -20);
            Gl.glVertex3d(-10, 10, -20);
            Gl.glVertex3d(-10, 0, -20);
            Gl.glEnd();

        }
        double[] CalcNormal(double[] p1, double[] p2, double[] p3)
        {
            double[] v1 = new double[]
            {
                p1[0] - p2[0],
                p1[1] - p2[1],
                p1[2] - p2[2]
            };
            double[] v2 = new double[]
            {
                p2[0] - p3[0],
                p2[1] - p3[1],
                p2[2] - p3[2]
            };
            return new double[]
            {
                v1[1] * v2[2] - v2[1] * v1[2],
                v2[0] * v1[2] - v1[0] * v2[2],
                v1[0] * v2[1] - v2[0] * v1[1]
            };
        }
        void mix()
        {

            /////----------------------------------
            /////Gl.glColor3d(1, 1, 1);

            //Gl.glEnable(Gl.GL_TEXTURE_2D);

            //Gl.glBindTexture(Gl.GL_TEXTURE_2D, treeTexture);

            //Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);


            //Gl.glEnable(Gl.GL_ALPHA_TEST);

            //Gl.glAlphaFunc(Gl.GL_GREATER, 0);

            //Gl.glBegin(Gl.GL_QUADS);


            //Gl.glTexCoord2d(0, 1);
            //Gl.glVertex3d(0, 0, 0);
            //Gl.glTexCoord2d(0, 0);
            //Gl.glVertex3d(0, 10, 0);
            //Gl.glTexCoord2d(1, 0);
            //Gl.glVertex3d(5, 10, 0);
            //Gl.glTexCoord2d(1, 1);
            //Gl.glVertex3d(5, 0, 0);

            //Gl.glEnd();

            //Gl.glTranslated(2.5, 0, 0);
            //Gl.glRotated(90, 0, 1, 0);
            //Gl.glTranslated(-2.5, 0, 0);
            //Gl.glBegin(Gl.GL_QUADS);


            //Gl.glTexCoord2d(0, 1);
            //Gl.glVertex3d(0, 0, 0);
            //Gl.glTexCoord2d(0, 0);
            //Gl.glVertex3d(0, 10, 0);
            //Gl.glTexCoord2d(1, 0);
            //Gl.glVertex3d(5, 10, 0);
            //Gl.glTexCoord2d(1, 1);
            //Gl.glVertex3d(5, 0, 0);

            //Gl.glEnd();
            //Gl.glDisable(Gl.GL_TEXTURE_2D);

            ////Gl.glTranslated(0, 0, -10);
            ////Gl.glColor3d(1, 0, 0);
            ////Gl.glRectd(-2, -2, 2, 2);

            ////Gl.glEnable(Gl.GL_BLEND);
            ////Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            //Gl.glEnable(Gl.GL_LINE_SMOOTH);
            //Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);

            //////Gl.glTranslated(0, 0, 4);
            //////Gl.glColor4d(0, 1, 0, 0.2);
            //////Gl.glRectd(0, 0, 2, 2);

            ////Gl.glLineWidth(10);
            //Gl.glTranslated(0, 0, -5);
            //Gl.glRotated(t, 0, 0, 1);
            //t += 0.5;
            //Gl.glBegin(Gl.GL_LINES);

            //Gl.glVertex3d(-2, 0, 0);
            //Gl.glVertex3d(2, 0, 0);
            //Gl.glEnd();


            //double t = 0, w, r = 100;
            //double x, z;
            //double t;
            //private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
            //{
            //    Gl.glEnable(Gl.GL_DEPTH_TEST);
            //    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            //    Gl.glLoadIdentity();

            //    Point[] points = new Point[]
            //    {
            //        new Point{X = 1,Y= -1,Z =1},
            //        new Point{X = 1,Y= 1,Z =1},
            //        new Point{X = -1,Y= 1,Z =1},
            //        new Point{X = -1,Y= -1,Z =1},
            //        new Point{X = 1,Y= -1,Z =-1},
            //        new Point{X = 1,Y= 1,Z =-1},
            //        new Point{X = -1,Y= 1,Z =-1},
            //        new Point{X = -1,Y= -1,Z =-1}
            //    };

            //    int[,] faces = new int[,] {
            //        {0,3,2,1},
            //        {0,1,5,4},
            //        {7,6,2,3},
            //        {7,4,5,6},
            //        {0,4,7,3},
            //        {1,2,6,5}
            //    };

            //    double[,] normals = new double[,]
            //    {
            //        {0,0,-1},
            //        {-1,0,0},
            //        {1,0,0},
            //        {0,0,1},
            //        {0,1,0},
            //        {0,-1,0}
            //    };


            //    Glu.gluLookAt(0, -1, 0, Math.Cos(t), 0, Math.Sin(t), 0, 1, 0);

            //    t += 0.1;


            //    Gl.glEnable(Gl.GL_LIGHTING);
            //    Gl.glEnable(Gl.GL_LIGHT0);

            //    Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            //    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { 0, 0, -1, 1 });

            //    Gl.glColor3d(1, 0, 0);

            //    //Gl.glTranslated(0, 0, -5);
            //    //Gl.glRotated(30, 1, 1, 0);
            //    Gl.glScaled(50, 50, 50);

            //    for (int i = 0; i < faces.GetLength(0); i++)
            //    {
            //        Gl.glNormal3d(normals[i, 0], normals[i, 1], normals[i, 2]);
            //        Gl.glColor3d(7.0 / i, 7.0 / i, 7.0 / i);
            //        Gl.glBegin(Gl.GL_QUADS);
            //        for (int j = 0; j < 4; j++)
            //        {
            //            Gl.glVertex3d(points[faces[i, j]].X, points[faces[i, j]].Y, points[faces[i, j]].Z);
            //        }
            //        Gl.glEnd();
            //    }
            //}
        }
        //private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        //{
        //    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        //    Gl.glLoadIdentity();

        //if (keydown.Contains(Keys.Left))
        //    t -= 0.1;
        //if (keydown.Contains(Keys.Right))
        //    t += 0.1;
        //if (keydown.Contains(Keys.Up))
        //{
        //    x += Math.Cos(t);
        //    z += Math.Sin(t);
        //}
        //if (keydown.Contains(Keys.Down))
        //{
        //    x -= Math.Cos(t);
        //    z -= Math.Sin(t);
        //}
        //if (keydown.Contains(Keys.W))
        //    w += 0.1;
        //if (keydown.Contains(Keys.S))
        //    w -= 0.1;

        //Glu.gluLookAt(x, 5, z, x + Math.Cos(t), 5 + Math.Sin(w), z + Math.Sin(t), 0, 1, 0);

        //for (int i = 0; i < 50; i++)
        //    for (int j = 0; j < 50; j++)
        //    {
        //        Gl.glColor3d((i + j) % 2, (i + j) % 2, (i + j) % 2);
        //        Gl.glBegin(Gl.GL_QUADS);
        //        Gl.glVertex3d(i * 10 - 250, 0, j * 10 - 250);
        //        Gl.glVertex3d(i * 10 - 250 + 10, 0, j * 10 - 250);
        //        Gl.glVertex3d(i * 10 - 250 + 10, 0, j * 10 - 250 + 10);
        //        Gl.glVertex3d(i * 10 - 250, 0, j * 10 - 250 + 10);
        //        Gl.glEnd();
        //    }

        //double t;

        //private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        //{
        //    Gl.glEnable(Gl.GL_DEPTH_TEST);

        //    Gl.glEnable(Gl.GL_LIGHTING);
        //    Gl.glEnable(Gl.GL_LIGHT0);
        //    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, new float[] { 1, 1, 1, 0 });
        //    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
        //    Gl.glEnable(Gl.GL_COLOR_MATERIAL);

        //    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
        //    Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, 125);

        //    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        //    Gl.glLoadIdentity();

        //    Gl.glTranslated(0, 0, -11);

        //    Gl.glColor3d(1, 0, 0);

        //    t += 1;

        //    Gl.glRotated(t, 0, 1, 0);

        //    //Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { (float)Math.Cos(t) * 5, 0, (float)Math.Sin(t) * 5, 1 });

        //    //Glut.glutSolidSphere(2, 30, 30);


        //    //Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, 1);

        //    Gl.glBegin(Gl.GL_TRIANGLES);
        //    Gl.glNormal3d(0, 0, 1);
        //    Gl.glVertex3d(-3, -2, 2);
        //    Gl.glVertex3d(3, -2, 2);
        //    Gl.glVertex3d(0, 4, 2);



        //    Gl.glNormal3d(0, 0, -1);

        //    Gl.glVertex3d(-3, -2, 0);
        //    Gl.glVertex3d(0, 4, 0);
        //    Gl.glVertex3d(3, -2, 0);



        //    Gl.glEnd();


        //}


        //}

        //double t1, t2, r = 5;
        //double t;
        //private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        //{
        //    Gl.glEnable(Gl.GL_DEPTH_TEST);

        //    Gl.glEnable(Gl.GL_LIGHTING);
        //    Gl.glEnable(Gl.GL_LIGHT0);
        //    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, new float[] { 1, 1, 1, 0 });
        //    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
        //    Gl.glEnable(Gl.GL_COLOR_MATERIAL);

        //    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, new float[] { 1, 1, 1, 0 });
        //    Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, 60);

        //    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        //    Gl.glLoadIdentity();

        //    if (keydown.Contains(Keys.Left))
        //        t1 -= 0.1;
        //    if (keydown.Contains(Keys.Right))
        //        t1 += 0.1;
        //    if (keydown.Contains(Keys.Up))
        //    {
        //        t2 += 0.1;
        //    }
        //    if (keydown.Contains(Keys.Down))
        //    {
        //        t2 -= 0.1;
        //    }
        //    if (keydown.Contains(Keys.W))
        //        r += 1;
        //    if (keydown.Contains(Keys.S))
        //        r -= 1;

        //    double x = Math.Cos(t1) * Math.Cos(t2) * r;
        //    double y = Math.Sin(t2) * r;
        //    double z = Math.Sin(t1) * Math.Cos(t2) * r;
        //    Glu.gluLookAt(x, y, z, 0, 0, 0, 0, 1, 0);



        //    Random rr = new Random();


        //    Gl.glDisable(Gl.GL_LIGHTING);
        //    Gl.glBegin(Gl.GL_POINTS);
        //    for (int i = 0; i < 100000; i++)
        //    {
        //        Gl.glVertex3d(rr.NextDouble() * 2000 - 1000, rr.NextDouble() * 2000 - 1000, -100);

        //    }
        //    Gl.glEnd();
        //    Gl.glEnable(Gl.GL_LIGHTING);



        //    Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { 0, 0, 0, 1 });

        //    Gl.glColor3d(1, 1, 0);

        //    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, new float[] { 1, 1, 0, 0 });
        //    Glut.glutSolidSphere(3, 30, 30);
        //    Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, new float[] { 0, 0, 0, 0 });
        //    double[] dis = { 5, 10, 12, 18, 23, 28, 35 };
        //    double[] rad = { 0.2, 0.8, 0.7, 0.6, 1.5, 0.3, 0.6 };
        //    double[] speed = { 1, 0.5, 0.3, 0.7, 1.2, 0.1, 0.6 };
        //    Color[] c = {Color.Red,Color.Gray,Color.Blue,Color.Green,
        //                     Color.Orange,Color.Pink,Color.White};

        //    for (int i = 0; i < dis.Length; i++)
        //    {

        //        Gl.glBegin(Gl.GL_LINE_LOOP);
        //        for (double tt = 0; tt <= Math.PI * 2; tt += 0.1)
        //        {
        //            Gl.glNormal3d(Math.Cos(tt), 0, Math.Sin(tt));
        //            Gl.glVertex3d(Math.Cos(tt) * dis[i], 0, Math.Sin(tt) * dis[i]);
        //        }
        //        Gl.glEnd();
        //        Gl.glPushMatrix();
        //        Gl.glColor3d(c[i].R / 255.0, c[i].G / 255.0, c[i].B / 255.0);
        //        Gl.glRotated(t * speed[i], 0, 1, 0);
        //        Gl.glTranslated(dis[i], 0, 0);
        //        Glut.glutSolidSphere(rad[i], 30, 30);
        //        Gl.glPopMatrix();
        //    }
        //    t += 2;
        //}

        //private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        //{
        //    Gl.glEnable(Gl.GL_DEPTH_TEST);
        //    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        //    Gl.glLoadIdentity();


        //    Gl.glBegin(Gl.GL_QUADS);
        //    Gl.glColor3d(1, 0, 0);
        //    Gl.glVertex3d(-1, -1, -4);
        //    Gl.glVertex3d(-1, 1, -4);
        //    Gl.glVertex3d(1, 1, -4);
        //    Gl.glVertex3d(1, -1, -4);

        //    Gl.glColor3d(0, 1, 0);
        //    Gl.glVertex3d(0, 0, -6);
        //    Gl.glVertex3d(0, 2, -6);
        //    Gl.glVertex3d(2, 2, -6);
        //    Gl.glVertex3d(2, 0, -6);

        //    Gl.glEnd();
        //}

        //double x, z;
        //double t;

        //private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        //{
        //    Gl.glEnable(Gl.GL_DEPTH_TEST);

        //    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
        //    Gl.glLoadIdentity();

        //    if (keydown.Contains(Keys.Up))
        //    {
        //        x += Math.Cos(t);
        //        z += Math.Sin(t);
        //    }
        //    if (keydown.Contains(Keys.Down))
        //    {
        //        x -= Math.Cos(t);
        //        z -= Math.Sin(t);
        //    }
        //    if (keydown.Contains(Keys.Left))
        //    {
        //        t -= 0.1;
        //    }
        //    if (keydown.Contains(Keys.Right))
        //    {
        //        t += 0.1;
        //    }

        //    Glu.gluLookAt(x, 5, z, x + Math.Cos(t), 5, z + Math.Sin(t), 0, 1, 0);

        //    Gl.glBegin(Gl.GL_QUADS);
        //    for (int i = -250; i < 250; i += 10)
        //        for (int j = -250; j < 250; j += 10)
        //        {
        //            int c = Math.Abs((i / 10 + j / 10) % 2);
        //            Gl.glColor3d(c, c, c);
        //            Gl.glVertex3d(i, 0, j);
        //            Gl.glColor3d(1 - c, 1 - c, 1 - c);
        //            Gl.glVertex3d(i + 10, 0, j);
        //            Gl.glColor3d(c, c, c);
        //            Gl.glVertex3d(i + 10, 0, j + 10);
        //            //Gl.glColor3d(1 - c, 1 - c, 1 - c);
        //            Gl.glVertex3d(i, 0, j + 10);
        //        }
        //    Gl.glEnd();
        //}

        void redrawThread()
        {
            while (true)
            {
                Thread.Sleep(50);
                simpleOpenGlControl1.Invoke(new Action(delegate()
                {
                    simpleOpenGlControl1.Draw();
                }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(redrawThread);
            t.IsBackground = true;
            t.Start();
        }


        class Point
        {
            double x, y, z;

            public double Z
            {
                get { return z; }
                set { z = value; }
            }

            public double Y
            {
                get { return y; }
                set { y = value; }
            }

            public double X
            {
                get { return x; }
                set { x = value; }
            }
        }

        int LoadTexture(String filename)
        {
            int id;
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glGenTextures(1, out id);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);
            int w, h;
            byte[] imageData = LoadImage(filename, out w, out h);
            //Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, imageData);
            Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGBA, w, h, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, imageData);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            return id;
        }

        int LoadTextureWithMask(String filename, String maskFilename)
        {
            int id;
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glGenTextures(1, out id);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, id);
            int w, h;
            byte[] imageData = LoadImageWithMask(filename, maskFilename, out w, out h);
            Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGBA, w, h, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, imageData);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            return id;
        }

        byte[] LoadImage(String filename, out int width, out int height)
        {
            Bitmap b = new Bitmap(filename);
            BitmapData d = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly,
                b.PixelFormat);
            byte[] data = new byte[b.Width * b.Height * 3];
            Marshal.Copy(d.Scan0, data, 0, data.Length);
            b.UnlockBits(d);
            for (int i = 0; i < data.Length; i += 3)
            {
                byte bb = data[i];
                data[i] = data[i + 2];
                data[i + 2] = bb;
            }
            width = b.Width;
            height = b.Height;
            return data;

        }

        int[,] heights;

        void drawHeightMap(String filename, int x, int z)
        {
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, grass);
            int w, h;
            byte[] data = LoadImage(filename, out w, out h);
            heights = new int[h, w];
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    heights[i, j] = data[(i * w + j) * 3];
            Gl.glBegin(Gl.GL_TRIANGLES);
            for (int i = x; i < x + h - 1; i++)
                for (int j = z; j < z + w - 1; j++)
                {
                    Gl.glTexCoord2d((i - x) / 20, (j - z) / 20);
                    Gl.glVertex3d(i, heights[i - x, j - z], j);
                    Gl.glTexCoord2d((i - x) / 20, (j + 1 - z) / 20);
                    Gl.glVertex3d(i, heights[i - x, j + 1 - z], j + 1);
                    Gl.glTexCoord2d((i + 1 - x) / 20, (j - z) / 20);
                    Gl.glVertex3d(i + 1, heights[i + 1 - x, j - z], j);

                    Gl.glTexCoord2d((i - x) / 20, (j + 1 - z) / 20);
                    Gl.glVertex3d(i, heights[i - x, j + 1 - z], j + 1);
                    Gl.glTexCoord2d((i + 1 - x) / 20, (j + 1 - z) / 20);
                    Gl.glVertex3d(i + 1, heights[i + 1 - x, j + 1 - z], j + 1);
                    Gl.glTexCoord2d((i + 1 - x) / 20, (j - z) / 20);
                    Gl.glVertex3d(i + 1, heights[i + 1 - x, j - z], j);
                }
            Gl.glEnd();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
        }

        byte[] LoadImageWithMask(String filename, String maskFilename, out int width, out int height)
        {
            int w, h;
            byte[] idata = LoadImage(filename, out w, out h);
            byte[] mdata = LoadImage(maskFilename, out w, out h);
            byte[] data = new byte[w * h * 4];
            for (int i = 0; i < w * h; i++)
            {
                data[i * 4] = idata[i * 3];
                data[i * 4 + 1] = idata[i * 3 + 1];
                data[i * 4 + 2] = idata[i * 3 + 2];

                if (mdata[i * 3] < 10 && mdata[i * 3 + 1] < 10 && mdata[i * 3 + 2] < 10)
                    data[i * 4 + 3] = 0;
                else
                    data[i * 4 + 3] = 1;
            }
            width = w;
            height = h;
            return data;
        }
      
        private void simpleOpenGlControl1_Resize(object sender, EventArgs e)
        {
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, simpleOpenGlControl1.Width / (double)simpleOpenGlControl1.Height, 0.1, 100000);
            Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
        }

        HashSet<Keys> keydown = new HashSet<Keys>();

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!keydown.Contains(keyData))
                keydown.Add(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void simpleOpenGlControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keydown.Contains(e.KeyCode))
                keydown.Add(e.KeyCode);
        }

        private void simpleOpenGlControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (keydown.Contains(e.KeyCode))
                keydown.Remove(e.KeyCode);
        }
    }
}
