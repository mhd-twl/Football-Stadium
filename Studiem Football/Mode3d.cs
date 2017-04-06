using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tao.OpenGl;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TAOSample
{
    class Mode3d
    {
        //>------ Primary Chunk, at the beginning of each file
        private const int PRIMARY = 0x4D4D;

        //>------ Main Chunks
        private const int OBJECTINFO = 0x3D3D;				// This gives the version of the mesh and is found right before the material and object information
        private const int VERSION = 0x0002;				// This gives the version of the .3ds file
        private const int EDITKEYFRAME = 0xB000;				// This is the header for all of the key frame info

        //>------ sub defines of OBJECTINFO
        private const int MATERIAL = 0xAFFF;				// This stored the texture info
        private const int OBJECT = 0x4000;				// This stores the faces, vertices, etc...

        //>------ sub defines of MATERIAL
        private const int MATNAME = 0xA000;				// This holds the material name
        private const int MATDIFFUSE = 0xA020;				// This holds the color of the object/material
        private const int MATMAP = 0xA200;				// This is a header for a new material
        private const int MATMAPFILE = 0xA300;				// This holds the file name of the texture

        private const int OBJECT_MESH = 0x4100;				// This lets us know that we are reading a new object

        //>------ sub defines of OBJECT_MESH
        private const int OBJECT_VERTICES = 0x4110;			// The objects vertices
        private const int OBJECT_FACES = 0x4120;			// The objects faces
        private const int OBJECT_MATERIAL = 0x4130;			// This is found if the object has a material, either texture map or color
        private const int OBJECT_UV = 0x4140;			// The UV texture coordinates

        bool oldVersion = false;

        public bool OldVersion
        {
            get { return oldVersion; }
            set { oldVersion = value; }
        }

        class tIndices
        {
            public ushort a, b, c, bVisible;		// This will hold point1, 2, and 3 index's into the vertex array plus a visible flag
        };

        // This holds the chunk info
        class tChunk
        {
            public ushort ID;					// The chunk's ID		
            public int length;					// The length of the chunk
            public int bytesRead;					// The amount of bytes read within that chunk
        };

        // This is our 3D point class.  This will be used to store the vertices of our model.
        class CVector3
        {
            public float x, y, z;
        };

        // This is our 2D point class.  This will be used to store the UV coordinates.
        class CVector2
        {
            public float x, y;
        };


        // This is our face structure.  This is is used for indexing into the vertex 
        // and texture coordinate arrays.  From this information we know which vertices
        // from our vertex array go to which face, along with the correct texture coordinates.
        class tFace
        {
            public int[] vertIndex = new int[3];			// indicies for the verts that make up this trian
            public int[] coordIndex = new int[3];			// indicies for the tex coords to texture this face
            public int material;
        }

        // This holds the information for a material.  It may be a texture map of a color.
        // Some of these are not used, but I left them because you will want to eventually
        // read in the UV tile ratio and the UV tile offset for some models.
        class tMaterialInfo
        {
            public String strName;			// The texture name
            public String strFile;			// The texture file name (If this is set it's a texture map)
            public byte[] color = new byte[3];				// The color of the object (R, G, B)
            public int texureId;				// the texture ID
            public float uTile;				// u tiling of texture  (Currently not used)
            public float vTile;				// v tiling of texture	(Currently not used)
            public float uOffset;			    // u offset of texture	(Currently not used)
            public float vOffset;				// v offset of texture	(Currently not used)
        }

        // This holds all the information for our model/scene. 
        // You should eventually turn into a robust class that 
        // has loading/drawing/querying functions like:
        // LoadModel(...); DrawObject(...); DrawModel(...); DestroyModel(...);
        class t3DObject
        {
            public int numOfVerts;			// The number of verts in the model
            public int numOfFaces;			// The number of faces in the model
            public int numTexVertex;			// The number of texture coordinates
            public int materialID;			// The texture ID to use, which is the index into our texture array
            public bool bHasTexture;			// This is TRUE if there is a texture map for this object
            public String strName;			// The name of the object
            public CVector3[] pVerts;			// The object's vertices
            public CVector3[] pNormals;		// The object's normals
            public CVector2[] pTexVerts;		// The texture's UV coordinates
            public tFace[] pFaces;				// The faces information of the object
        }

        // This holds our model information.  This should also turn into a robust class.
        // We use STL's (Standard Template Library) vector class to ease our link list burdens. :)
        class t3DModel
        {
            public int numOfObjects;					// The number of objects in the model
            public int numOfMaterials;					// The number of materials for the model
            public List<tMaterialInfo> pMaterials = new List<tMaterialInfo>();	// The list of material information (Textures and colors)
            public List<t3DObject> pObject = new List<t3DObject>();			// The object list for our model
        }

        public Mode3d()								// This inits the data members
        {
        }

        t3DModel pModel; //the current model to be loaded
        List<int> g_Texture = new List<int>();


        public void Draw_3DS_Object(double pX, double pY, double pZ, double pSize)
        {
            // We want the model to rotate around the axis so we give it a rotation
            // value, then increase/decrease it. You can rotate right of left with the arrow keys.
            Gl.glPushMatrix();

            Gl.glTranslated(pX, pY, pZ);
            Gl.glScaled(pSize, pSize, pSize);


            // We have a model that has a certain amount of objects and textures.  We want to go through each object 
            // in the model, bind it's texture map to it, then render it by going through all of it's faces (Polygons).  	

            // Since we know how many objects our model has, go through each of them.
            for (int i = 0; i < pModel.numOfObjects; i++)
            {
                // Make sure we have valid objects just in case. (size() is in the vector class)
                if (pModel.pObject.Count <= 0) break;

                // Get the current object that we are displaying
                t3DObject pObject = pModel.pObject[i];

                // Check to see if this object has a texture map, if so bind the texture to it.
                if (pObject.bHasTexture && pObject.materialID < g_Texture.Count)
                {
                    // Turn on texture mapping and turn off color
                    Gl.glEnable(Gl.GL_TEXTURE_2D);

                    // Reset the color to normal again
                    Gl.glColor3ub(255, 255, 255);

                    // Bind the texture map to the object by it's materialID
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, g_Texture[pObject.materialID]);
                }
                else
                {
                    // Turn off texture mapping and turn on color
                    Gl.glDisable(Gl.GL_TEXTURE_2D);

                    // Reset the color to normal again
                    Gl.glColor3ub(255, 255, 255);
                }

                // This determines if we are in wireframe or normal mode
                Gl.glBegin(Gl.GL_TRIANGLES);					// Begin drawing with our selected mode (triangles or lines)

                // Go through all of the faces (polygons) of the object and draw them
                for (int j = 0; j < pObject.numOfFaces; j++)
                {
                    // Go through each corner of the triangle and draw it.
                    for (int whichVertex = 0; whichVertex < 3; whichVertex++)
                    {
                        // Get the index for each point of the face
                        int index = pObject.pFaces[j].vertIndex[whichVertex];

                        // Give OpenGL the normal for this vertex.
                        Gl.glNormal3f(pObject.pNormals[index].x, pObject.pNormals[index].y, pObject.pNormals[index].z);

                        // If the object has a texture associated with it, give it a texture coordinate.
                        if (pObject.bHasTexture)
                        {

                            // Make sure there was a UVW map applied to the object or else it won't have tex coords.
                            if (pObject.pTexVerts != null)
                            {
                                Gl.glTexCoord2f(pObject.pTexVerts[index].x, pObject.pTexVerts[index].y);
                            }
                        }
                        else
                        {
                            // if the size is at > 1 and material ID != -1, then it is a valid material.
                            if (pModel.pMaterials.Count > 0 && pObject.materialID >= 0)
                            {
                                byte[] pColor = null;
                                if (!oldVersion)
                                    // Get and set the color that the object is, since it must not have a texture
                                    pColor = pModel.pMaterials[pObject.materialID].color;
                                else
                                    pColor = pModel.pMaterials[pObject.pFaces[j].material].color;
                                // Assign the current color to this model
                                Gl.glColor3ub(pColor[0], pColor[1], pColor[2]);
                            }
                        }

                        // Pass in the current vertex of the object (Corner of current face)
                        Gl.glVertex3f(pObject.pVerts[index].x, pObject.pVerts[index].y, pObject.pVerts[index].z);
                    }
                }

                Gl.glEnd();			// End the model drawing
            }

            Gl.glPopMatrix();
        }

        byte[] LoadImage(String filename, out int width, out int height)
        {
            try
            {
                if (filename.EndsWith("\0"))
                    filename = filename.Substring(0, filename.Length - 1);
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
            catch
            {
                width = 5;
                height = 2;
                byte[] data = new byte[10];
                for (int i = 0; i < data.Length; i++)
                    data[i] = 255;
                return data;
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

        // This is the function that you call to load the 3DS
        public bool Import3DS(String strFileName)
        {
            String strMessage = "";
            pModel = new t3DModel();
            // Open the 3DS file
            m_FilePointer = new FileStream(strFileName, FileMode.Open);

            // Once we have the file open, we need to read the very first data chunk
            // to see if it's a 3DS file.  That way we don't read an invalid file.
            // If it is a 3DS file, then the first chunk ID will be equal to PRIMARY (some hex num)

            // Read the first chuck of the file to see if it's a 3DS file
            ReadChunk(m_CurrentChunk);

            // Make sure this is a 3DS file
            if (m_CurrentChunk.ID != PRIMARY)
            {
                throw new Exception("Unable to load PRIMARY chuck from file:" + strFileName);
            }

            // Now we actually start reading in the data.  ProcessNextChunk() is recursive

            // Begin loading objects, by calling this recursive function
            ProcessNextChunk(pModel, m_CurrentChunk);

            // After we have read the whole 3DS file, we want to calculate our own vertex normals.
            ComputeNormals(pModel);

            // Clean up after everything
            CleanUp();

            //load textures:
            // Depending on how many textures we found, load each one
            for (int i = 0; i < pModel.numOfMaterials; i++)
            {
                // Check to see if there is a file name to load in this material
                if (pModel.pMaterials[i].strFile != null)
                {
                    // We pass in our global texture array, the name of the texture, and an ID to reference it.	
                    g_Texture.Add(LoadTexture(pModel.pMaterials[i].strFile));
                }

                // Set the texture ID for this material
                pModel.pMaterials[i].texureId = i;
            }

            return true;
        }

        // This reads in a string and saves it in the char array passed in
        String GetString(out int length)
        {
            byte[] buffer = new byte[10000];
            int c = 0;
            buffer[0] = (byte)m_FilePointer.ReadByte();
            for (; buffer[c] != 0; )
            {
                buffer[++c] = (byte)m_FilePointer.ReadByte();
            }
            String result = Encoding.ASCII.GetString(buffer, 0, c);
            length = c + 1;
            return result;
        }

        // This reads the next chunk
        void ReadChunk(tChunk pChunk)
        {
            // This reads the chunk ID which is 2 bytes.
            // The chunk ID is like OBJECT or MATERIAL.  It tells what data is
            // able to be read in within the chunks section.  
            BinaryReader reader = new BinaryReader(m_FilePointer);
            pChunk.ID = reader.ReadUInt16();
            pChunk.bytesRead = 2;

            // Then, we read the length of the chunk which is 4 bytes.
            // This is how we know how much to read in, or read past.
            pChunk.length = reader.ReadInt32();
            pChunk.bytesRead += 4;
        }


        void ProcessNextChunk(t3DModel pModel, tChunk pPreviousChunk)
        {
            t3DObject newObject = new t3DObject();					// This is used to add to our object list
            tMaterialInfo newTexture = new tMaterialInfo();				// This is used to add to our material list
            uint version = 0;					// This will hold the file version
            byte[] buffer = new byte[5000000];					// This is used to read past unwanted data

            tChunk m_CurrentChunk = new tChunk();				// Allocate a new chunk				

            // Below we check our chunk ID each time we read a new chunk.  Then, if
            // we want to extract the information from that chunk, we do so.
            // If we don't want a chunk, we just read past it.  

            // Continue to read the sub chunks until we have reached the length.
            // After we read ANYTHING we add the bytes read to the chunk and then check
            // check against the length.
            while (pPreviousChunk.bytesRead < pPreviousChunk.length)
            {
                // Read next Chunk
                ReadChunk(m_CurrentChunk);

                // Check the chunk ID
                switch (m_CurrentChunk.ID)
                {
                    case VERSION:							// This holds the version of the file

                        // This chunk has an unsigned short that holds the file version.
                        // Since there might be new additions to the 3DS file format in 4.0,
                        // we give a warning to that problem.

                        // Read the file version and add the bytes read to our bytesRead variable
                        m_FilePointer.Read(buffer, 0, m_CurrentChunk.length - m_CurrentChunk.bytesRead);
                        m_CurrentChunk.bytesRead += 4;
                        //TODO manage version
                        break;
                    case OBJECTINFO:						// This holds the version of the mesh

                        // This chunk holds the version of the mesh.  It is also the head of the MATERIAL
                        // and OBJECT chunks.  From here on we start reading in the material and object info.

                        // Read the next chunk
                        ReadChunk(m_TempChunk);

                        // Get the version of the mesh
                        m_TempChunk.bytesRead += m_FilePointer.Read(new byte[10000], 0, m_TempChunk.length - m_TempChunk.bytesRead);

                        // Increase the bytesRead by the bytes read from the last chunk
                        m_CurrentChunk.bytesRead += m_TempChunk.bytesRead;

                        // Go to the next chunk, which is the object has a texture, it should be MATERIAL, then OBJECT.
                        ProcessNextChunk(pModel, m_CurrentChunk);
                        break;

                    case MATERIAL:							// This holds the material information

                        // This chunk is the header for the material info chunks

                        // Increase the number of materials
                        pModel.numOfMaterials++;

                        // Add a empty texture structure to our texture list.
                        // If you are unfamiliar with STL's "vector" class, all push_back()
                        // does is add a new node onto the list.  I used the vector class
                        // so I didn't need to write my own link list functions.  
                        newTexture = new tMaterialInfo();
                        pModel.pMaterials.Add(newTexture);

                        // Proceed to the material loading function
                        ProcessNextMaterialChunk(pModel, m_CurrentChunk);
                        break;

                    case OBJECT:							// This holds the name of the object being read

                        // This chunk is the header for the object info chunks.  It also
                        // holds the name of the object.

                        // Increase the object count
                        pModel.numOfObjects++;

                        // Add a new tObject node to our list of objects (like a link list)
                        newObject = new t3DObject();
                        pModel.pObject.Add(newObject);

                        // Get the name of the object and store it, then add the read bytes to our byte counter.
                        int r;
                        pModel.pObject[pModel.numOfObjects - 1].strName = GetString(out r);
                        m_CurrentChunk.bytesRead += r;

                        // Now proceed to read in the rest of the object information
                        ProcessNextObjectChunk(pModel, (pModel.pObject[pModel.numOfObjects - 1]), m_CurrentChunk);
                        break;

                    case EDITKEYFRAME:

                        // Because I wanted to make this a SIMPLE tutorial as possible, I did not include
                        // the key frame information.  This chunk is the header for all the animation info.
                        // In a later tutorial this will be the subject and explained thoroughly.

                        //ProcessNextKeyFrameChunk(pModel, m_CurrentChunk);

                        // Read past this chunk and add the bytes read to the byte counter
                        m_CurrentChunk.bytesRead += m_FilePointer.Read(new byte[100000], 0, m_CurrentChunk.length - m_CurrentChunk.bytesRead);
                        break;

                    default:

                        // If we didn't care about a chunk, then we get here.  We still need
                        // to read past the unknown or ignored chunk and add the bytes read to the byte counter.
                        m_CurrentChunk.bytesRead += m_FilePointer.Read(new byte[100000], 0, m_CurrentChunk.length - m_CurrentChunk.bytesRead);
                        break;
                }

                // Add the bytes read from the last chunk to the previous chunk passed in.
                pPreviousChunk.bytesRead += m_CurrentChunk.bytesRead;
            }
        }

        // This reads the object chunks
        void ProcessNextObjectChunk(t3DModel pModel, t3DObject pObject, tChunk pPreviousChunk)
        {
            byte[] buffer = new byte[500000];					// This is used to read past unwanted data

            // Allocate a new chunk to work with
            m_CurrentChunk = new tChunk();

            // Continue to read these chunks until we read the end of this sub chunk
            while (pPreviousChunk.bytesRead < pPreviousChunk.length)
            {
                // Read the next chunk
                ReadChunk(m_CurrentChunk);

                // Check which chunk we just read
                switch (m_CurrentChunk.ID)
                {
                    case OBJECT_MESH:					// This lets us know that we are reading a new object

                        // We found a new object, so let's read in it's info using recursion
                        ProcessNextObjectChunk(pModel, pObject, m_CurrentChunk);
                        break;

                    case OBJECT_VERTICES:				// This is the objects vertices
                        ReadVertices(pObject, m_CurrentChunk);
                        break;

                    case OBJECT_FACES:					// This is the objects face information
                        ReadVertexIndices(pObject, m_CurrentChunk);
                        break;

                    case OBJECT_MATERIAL:				// This holds the material name that the object has

                        // This chunk holds the name of the material that the object has assigned to it.
                        // This could either be just a color or a texture map.  This chunk also holds
                        // the faces that the texture is assigned to (In the case that there is multiple
                        // textures assigned to one object, or it just has a texture on a part of the object.
                        // Since most of my game objects just have the texture around the whole object, and 
                        // they aren't multitextured, I just want the material name.

                        // We now will read the name of the material assigned to this object
                        ReadObjectMaterial(pModel, pObject, m_CurrentChunk);
                        break;

                    case OBJECT_UV:						// This holds the UV texture coordinates for the object

                        // This chunk holds all of the UV coordinates for our object.  Let's read them in.
                        ReadUVCoordinates(pObject, m_CurrentChunk);
                        break;

                    default:

                        // Read past the ignored or unknown chunks
                        m_CurrentChunk.bytesRead += m_FilePointer.Read(buffer, 0, m_CurrentChunk.length - m_CurrentChunk.bytesRead);
                        break;
                }

                // Add the bytes read from the last chunk to the previous chunk passed in.
                pPreviousChunk.bytesRead += m_CurrentChunk.bytesRead;
            }

            // Free the current chunk and set it back to the previous chunk (since it started that way)
            m_CurrentChunk = null;
            m_CurrentChunk = pPreviousChunk;
        }

        // This reads the material chunks
        void ProcessNextMaterialChunk(t3DModel pModel, tChunk pPreviousChunk)
        {
            byte[] buffer = new byte[50000];					// This is used to read past unwanted data

            // Allocate a new chunk to work with
            m_CurrentChunk = new tChunk();

            // Continue to read these chunks until we read the end of this sub chunk
            while (pPreviousChunk.bytesRead < pPreviousChunk.length)
            {
                // Read the next chunk
                ReadChunk(m_CurrentChunk);

                // Check which chunk we just read in
                switch (m_CurrentChunk.ID)
                {
                    case MATNAME:							// This chunk holds the name of the material

                        // Here we read in the material name
                        int r = m_FilePointer.Read(buffer, 0, m_CurrentChunk.length - m_CurrentChunk.bytesRead);
                        pModel.pMaterials[pModel.numOfMaterials - 1].strName = Encoding.ASCII.GetString(buffer, 0, r - 1);
                        m_CurrentChunk.bytesRead += r;
                        break;

                    case MATDIFFUSE:						// This holds the R G B color of our object
                        ReadColorChunk((pModel.pMaterials[pModel.numOfMaterials - 1]), m_CurrentChunk);
                        break;

                    case MATMAP:							// This is the header for the texture info

                        // Proceed to read in the material information
                        ProcessNextMaterialChunk(pModel, m_CurrentChunk);
                        break;

                    case MATMAPFILE:						// This stores the file name of the material

                        // Here we read in the material's file name
                        int rr = m_FilePointer.Read(buffer, 0, m_CurrentChunk.length - m_CurrentChunk.bytesRead);
                        pModel.pMaterials[pModel.numOfMaterials - 1].strFile = Encoding.ASCII.GetString(buffer, 0, rr);
                        m_CurrentChunk.bytesRead += rr;
                        break;

                    default:

                        // Read past the ignored or unknown chunks
                        m_CurrentChunk.bytesRead += m_FilePointer.Read(buffer, 0, m_CurrentChunk.length - m_CurrentChunk.bytesRead);
                        break;
                }

                // Add the bytes read from the last chunk to the previous chunk passed in.
                pPreviousChunk.bytesRead += m_CurrentChunk.bytesRead;
            }

            // Free the current chunk and set it back to the previous chunk (since it started that way)
            m_CurrentChunk = pPreviousChunk;
        }

        // This reads the RGB value for the object's color
        void ReadColorChunk(tMaterialInfo pMaterial, tChunk pChunk)
        {
            // Read the color chunk info
            ReadChunk(m_TempChunk);

            // Read in the R G B color (3 bytes - 0 through 255)
            m_TempChunk.bytesRead += m_FilePointer.Read(pMaterial.color, 0, m_TempChunk.length - m_TempChunk.bytesRead);

            // Add the bytes read to our chunk
            pChunk.bytesRead += m_TempChunk.bytesRead;
        }

        // This reads the objects vertices
        void ReadVertices(t3DObject pObject, tChunk pPreviousChunk)
        {
            // Like most chunks, before we read in the actual vertices, we need
            // to find out how many there are to read in.  Once we have that number
            // we then fread() them into our vertice array.

            // Read in the number of vertices (int)
            BinaryReader reader = new BinaryReader(m_FilePointer);
            pObject.numOfVerts = reader.ReadUInt16();
            pPreviousChunk.bytesRead += 2;

            // Allocate the memory for the verts and initialize the structure
            pObject.pVerts = new CVector3[pObject.numOfVerts];

            // Read in the array of vertices (an array of 3 floats)
            for (int i = 0, j = 0; i < pPreviousChunk.length - pPreviousChunk.bytesRead; i += sizeof(float) * 3)
            {
                CVector3 v = new CVector3();
                v.x = reader.ReadSingle();
                v.y = reader.ReadSingle();
                v.z = reader.ReadSingle();
                pObject.pVerts[j++] = v;
            }
            pPreviousChunk.bytesRead += sizeof(float) * 3 * pObject.pVerts.Length;

            // Now we should have all of the vertices read in.  Because 3D Studio Max
            // Models with the Z-Axis pointing up (strange and ugly I know!), we need
            // to flip the y values with the z values in our vertices.  That way it
            // will be normal, with Y pointing up.  If you prefer to work with Z pointing
            // up, then just delete this next loop.  Also, because we swap the Y and Z
            // we need to negate the Z to make it come out correctly.

            // Go through all of the vertices that we just read and swap the Y and Z values
            for (int i = 0; i < pObject.numOfVerts; i++)
            {
                // Store off the Y value
                float fTempY = pObject.pVerts[i].y;

                // Set the Y value to the Z value
                pObject.pVerts[i].y = pObject.pVerts[i].z;

                // Set the Z value to the Y value, 
                // but negative Z because 3D Studio max does the opposite.
                pObject.pVerts[i].z = -fTempY;
            }
        }

        // This reads the objects face information
        void ReadVertexIndices(t3DObject pObject, tChunk pPreviousChunk)
        {
            ushort index = 0;					// This is used to read in the current face index

            // In order to read in the vertex indices for the object, we need to first
            // read in the number of them, then read them in.  Remember,
            // we only want 3 of the 4 values read in for each face.  The fourth is
            // a visibility flag for 3D Studio Max that doesn't mean anything to us.

            // Read in the number of faces that are in this object (int)
            BinaryReader reader = new BinaryReader(m_FilePointer);
            pObject.numOfFaces = reader.ReadUInt16();
            pPreviousChunk.bytesRead += 2;

            // Alloc enough memory for the faces and initialize the structure
            pObject.pFaces = new tFace[pObject.numOfFaces];


            // Go through all of the faces in this object
            for (int i = 0; i < pObject.numOfFaces; i++)
            {
                // Next, we read in the A then B then C index for the face, but ignore the 4th value.
                // The fourth value is a visibility flag for 3D Studio Max, we don't care about this.
                pObject.pFaces[i] = new tFace();
                for (int j = 0; j < 4; j++)
                {
                    // Read the first vertice index for the current face 
                    index = reader.ReadUInt16();
                    pPreviousChunk.bytesRead += 2;

                    if (j < 3)
                    {
                        // Store the index in our face structure.
                        pObject.pFaces[i].vertIndex[j] = index;
                    }
                    else
                        pObject.pFaces[i].material = index;
                }
            }
        }

        // This reads the texture coodinates of the object
        void ReadUVCoordinates(t3DObject pObject, tChunk pPreviousChunk)
        {
            // In order to read in the UV indices for the object, we need to first
            // read in the amount there are, then read them in.

            // Read in the number of UV coordinates there are (int)
            BinaryReader reader = new BinaryReader(m_FilePointer);
            pObject.numTexVertex = reader.ReadUInt16();
            pPreviousChunk.bytesRead += 2;

            // Allocate memory to hold the UV coordinates
            pObject.pTexVerts = new CVector2[pObject.numTexVertex];

            // Read in the texture coodinates (an array 2 float)
            for (int i = 0, j = 0; i < pPreviousChunk.length - pPreviousChunk.bytesRead; i += sizeof(float) * 2)
            {
                float f1 = reader.ReadSingle();
                float f2 = reader.ReadSingle();
                CVector2 c = new CVector2 { x = f1, y = f2 };
                pObject.pTexVerts[j++] = c;
            }
            pPreviousChunk.bytesRead += 2 * sizeof(float) * pObject.pTexVerts.Length;
        }

        // This reads in the material name assigned to the object and sets the materialID
        void ReadObjectMaterial(t3DModel pModel, t3DObject pObject, tChunk pPreviousChunk)
        {
            byte[] buffer = new byte[1000000];			// This is used to hold the objects material name
            String strMaterial;

            // *What is a material?*  - A material is either the color or the texture map of the object.
            // It can also hold other information like the brightness, shine, etc... Stuff we don't
            // really care about.  We just want the color, or the texture map file name really.

            // Here we read the material name that is assigned to the current object.
            // strMaterial should now have a string of the material name, like "Material #2" etc..
            int size;
            strMaterial = GetString(out size);
            pPreviousChunk.bytesRead += size;

            // Now that we have a material name, we need to go through all of the materials
            // and check the name against each material.  When we find a material in our material
            // list that matches this name we just read in, then we assign the materialID
            // of the object to that material index.  You will notice that we passed in the
            // model to this function.  This is because we need the number of textures.
            // Yes though, we could have just passed in the model and not the object too.

            // Go through all of the textures
            for (int i = 0; i < pModel.numOfMaterials; i++)
            {
                // If the material we just read in matches the current texture name
                if (strMaterial == pModel.pMaterials[i].strName)
                {
                    // Set the material ID to the current index 'i' and stop checking
                    pObject.materialID = i;

                    // Now that we found the material, check if it's a texture map.
                    // If the strFile has a string length of 1 and over it's a texture
                    if (pModel.pMaterials[i].strFile != null)
                    {

                        // Set the object's flag to say it has a texture map to bind.
                        pObject.bHasTexture = true;
                    }
                    break;
                }
                else
                {
                    // Set the ID to -1 to show there is no material for this object
                    pObject.materialID = -1;
                }
            }

            // Read past the rest of the chunk since we don't care about shared vertices
            // You will notice we subtract the bytes already read in this chunk from the total length.
            pPreviousChunk.bytesRead += m_FilePointer.Read(buffer, 0, pPreviousChunk.length - pPreviousChunk.bytesRead);
        }

        double Mag(CVector3 Normal) { return Math.Sqrt(Normal.x * Normal.x + Normal.y * Normal.y + Normal.z * Normal.z); }

        // This calculates a vector between 2 points and returns the result
        CVector3 Vector(CVector3 vPoint1, CVector3 vPoint2)
        {
            CVector3 vVector = new CVector3();							// The variable to hold the resultant vector

            vVector.x = vPoint1.x - vPoint2.x;			// Subtract point1 and point2 x's
            vVector.y = vPoint1.y - vPoint2.y;			// Subtract point1 and point2 y's
            vVector.z = vPoint1.z - vPoint2.z;			// Subtract point1 and point2 z's

            return vVector;								// Return the resultant vector
        }

        // This adds 2 vectors together and returns the result
        CVector3 AddVector(CVector3 vVector1, CVector3 vVector2)
        {
            CVector3 vResult = new CVector3();							// The variable to hold the resultant vector

            vResult.x = vVector2.x + vVector1.x;		// Add Vector1 and Vector2 x's
            vResult.y = vVector2.y + vVector1.y;		// Add Vector1 and Vector2 y's
            vResult.z = vVector2.z + vVector1.z;		// Add Vector1 and Vector2 z's

            return vResult;								// Return the resultant vector
        }

        // This divides a vector by a single number (scalar) and returns the result
        CVector3 DivideVectorByScaler(CVector3 vVector1, float Scaler)
        {
            CVector3 vResult = new CVector3();							// The variable to hold the resultant vector

            vResult.x = vVector1.x / Scaler;			// Divide Vector1's x value by the scaler
            vResult.y = vVector1.y / Scaler;			// Divide Vector1's y value by the scaler
            vResult.z = vVector1.z / Scaler;			// Divide Vector1's z value by the scaler

            return vResult;								// Return the resultant vector
        }

        // This returns the cross product between 2 vectors
        CVector3 Cross(CVector3 vVector1, CVector3 vVector2)
        {
            CVector3 vCross = new CVector3();								// The vector to hold the cross product
            // Get the X value
            vCross.x = ((vVector1.y * vVector2.z) - (vVector1.z * vVector2.y));
            // Get the Y value
            vCross.y = ((vVector1.z * vVector2.x) - (vVector1.x * vVector2.z));
            // Get the Z value
            vCross.z = ((vVector1.x * vVector2.y) - (vVector1.y * vVector2.x));

            return vCross;								// Return the cross product
        }

        // This returns the normal of a vector
        CVector3 Normalize(CVector3 vNormal)
        {
            double Magnitude;							// This holds the magitude			

            Magnitude = Mag(vNormal);					// Get the magnitude

            vNormal.x /= (float)Magnitude;				// Divide the vector's X by the magnitude
            vNormal.y /= (float)Magnitude;				// Divide the vector's Y by the magnitude
            vNormal.z /= (float)Magnitude;				// Divide the vector's Z by the magnitude

            return new CVector3 { x = vNormal.x, y = vNormal.y, z = vNormal.z };								// Return the normal
        }


        // This computes the vertex normals for the object (used for lighting)
        void ComputeNormals(t3DModel pModel)
        {
            CVector3 vVector1 = new CVector3(),
                vVector2 = new CVector3(),
                vNormal = new CVector3();
            CVector3[] vPoly = new CVector3[3];

            // If there are no objects, we can skip this part
            if (pModel.numOfObjects <= 0)
                return;

            // What are vertex normals?  And how are they different from other normals?
            // Well, if you find the normal to a triangle, you are finding a "Face Normal".
            // If you give OpenGL a face normal for lighting, it will make your object look
            // really flat and not very round.  If we find the normal for each vertex, it makes
            // the smooth lighting look.  This also covers up blocky looking objects and they appear
            // to have more polygons than they do.    Basically, what you do is first
            // calculate the face normals, then you take the average of all the normals around each
            // vertex.  It's just averaging.  That way you get a better approximation for that vertex.

            // Go through each of the objects to calculate their normals
            for (int index = 0; index < pModel.numOfObjects; index++)
            {
                // Get the current object
                t3DObject pObject = pModel.pObject[index];

                // Here we allocate all the memory we need to calculate the normals
                CVector3[] pNormals = new CVector3[pObject.numOfFaces];
                CVector3[] pTempNormals = new CVector3[pObject.numOfFaces];
                pObject.pNormals = new CVector3[pObject.numOfVerts];

                // Go though all of the faces of this object
                for (int i = 0; i < pObject.numOfFaces; i++)
                {
                    // To cut down LARGE code, we extract the 3 points of this face
                    vPoly[0] = pObject.pVerts[pObject.pFaces[i].vertIndex[0]];
                    vPoly[1] = pObject.pVerts[pObject.pFaces[i].vertIndex[1]];
                    vPoly[2] = pObject.pVerts[pObject.pFaces[i].vertIndex[2]];

                    // Now let's calculate the face normals (Get 2 vectors and find the cross product of those 2)

                    vVector1 = Vector(vPoly[0], vPoly[2]);		// Get the vector of the polygon (we just need 2 sides for the normal)
                    vVector2 = Vector(vPoly[2], vPoly[1]);		// Get a second vector of the polygon

                    vNormal = Cross(vVector1, vVector2);		// Return the cross product of the 2 vectors (normalize vector, but not a unit vector)
                    pTempNormals[i] = vNormal;					// Save the un-normalized normal for the vertex normals
                    vNormal = Normalize(vNormal);				// Normalize the cross product to give us the polygons normal

                    pNormals[i] = vNormal;						// Assign the normal to the list of normals
                }

                //////////////// Now Get The Vertex Normals /////////////////

                CVector3 vSum = new CVector3 { x = 0.0f, y = 0.0f, z = 0.0f };
                CVector3 vZero = new CVector3 { x = 0.0f, y = 0.0f, z = 0.0f };
                int shared = 0;

                for (int i = 0; i < pObject.numOfVerts; i++)			// Go through all of the vertices
                {
                    for (int j = 0; j < pObject.numOfFaces; j++)	// Go through all of the triangles
                    {												// Check if the vertex is shared by another face
                        if (pObject.pFaces[j].vertIndex[0] == i ||
                            pObject.pFaces[j].vertIndex[1] == i ||
                            pObject.pFaces[j].vertIndex[2] == i)
                        {
                            vSum = AddVector(vSum, pTempNormals[j]);// Add the un-normalized normal of the shared face
                            shared++;								// Increase the number of shared triangles
                        }
                    }

                    // Get the normal by dividing the sum by the shared.  We negate the shared so it has the normals pointing out.
                    pObject.pNormals[i] = DivideVectorByScaler(vSum, (float)(-shared));

                    // Normalize the normal for the final vertex normal
                    pObject.pNormals[i] = Normalize(pObject.pNormals[i]);

                    vSum = vZero;									// Reset the sum
                    shared = 0;										// Reset the shared
                }

            }
        }

        // This frees memory and closes the file
        void CleanUp()
        {
            m_FilePointer.Close();
        }

        // The file pointer
        FileStream m_FilePointer;

        // These are used through the loading process to hold the chunk information
        tChunk m_CurrentChunk = new tChunk();
        tChunk m_TempChunk = new tChunk();

    }
}
