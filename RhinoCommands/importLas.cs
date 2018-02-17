using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace RhinoCommands {
    public class ImportLasCommand : Command {
        public ImportLasCommand() {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static ImportLasCommand Instance {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName {
            get { return "ImportLas"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode) {
            // TODO: start here modifying the behaviour of your command.
            // ---
           if (!BitConverter.IsLittleEndian) { RhinoApp.WriteLine("Big Endian Error"); return Result.Failure; }
            RhinoApp.WriteLine("Importing .Las files", EnglishName);


            string userSelectedFilePath;
                char[] header = new char[4];
            string lasf;

            UInt32 pointStart = 0;
            UInt64 pointCount = 0;
            char versionMajor;
            char versionMinor;
            char pointRecordFormat;
            short pointRecordLength;

            double scaleX = 1.0;
            double scaleY = 1.0;
            double scaleZ = 1.0;

            double offsetX = 0.0;
            double offsetY = 0.0;
            double offsetZ = 0.0;

            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();

            Point4d[] points;
            int[] classifications;



            if (dr == System.Windows.Forms.DialogResult.OK) {
                userSelectedFilePath = ofd.FileName;
                byte[] buffer = System.IO.File.ReadAllBytes(userSelectedFilePath);

                //TODO add header check
                header[0] = (char)buffer[0];
                header[1] = (char)buffer[1];
                header[2] = (char)buffer[2];
                header[3] = (char)buffer[3];
                lasf = new string(header);


                versionMajor = (char)buffer[24];
                versionMinor = (char)buffer[25];

                
                pointStart = BitConverter.ToUInt32(buffer, 96);

                if (versionMinor >= '4') {
                    pointRecordFormat = (char)buffer[104];
                    pointRecordLength = BitConverter.ToInt16(buffer, 105);
                    pointCount = BitConverter.ToUInt64(buffer, 247);
                    scaleX = BitConverter.ToDouble(buffer, 131);
                    scaleY = BitConverter.ToDouble(buffer, 139);
                    scaleZ = BitConverter.ToDouble(buffer, 147);
                    offsetX = BitConverter.ToDouble(buffer, 155);
                    offsetY = BitConverter.ToDouble(buffer, 163);
                    offsetZ = BitConverter.ToDouble(buffer, 171);
                } else {
                    pointRecordFormat = (char)buffer[104];
                    pointRecordLength = BitConverter.ToInt16(buffer, 105);
                    pointCount = BitConverter.ToUInt32(buffer, 107);
                    scaleX = BitConverter.ToDouble(buffer, 131);
                    scaleY = BitConverter.ToDouble(buffer, 139);
                    scaleZ = BitConverter.ToDouble(buffer, 147);
                    offsetX = BitConverter.ToDouble(buffer, 155);
                    offsetY = BitConverter.ToDouble(buffer, 163);
                    offsetZ = BitConverter.ToDouble(buffer, 171);
                }

                points = new Point4d[pointCount];
                classifications = new int[pointCount];
                uint position = pointStart;
                for (int i = 0; i < points.Length; i++) {
                    double X, Y, Z;
                    int intensity;
                    long rawX, rawY, rawZ;
                    rawX = BitConverter.ToInt32(buffer, (int)position+0);
                    rawY = BitConverter.ToInt32(buffer, (int)position+4);
                    rawZ = BitConverter.ToInt32(buffer, (int)position+8);
                    X = (scaleX * rawX) + offsetX;
                    Y = (scaleY * rawY) + offsetY;
                    Z = (scaleZ * rawZ) + offsetZ;
                    intensity = BitConverter.ToInt16(buffer, (int)position + 12);
                    classifications[i] = (char)buffer[position + 22];

                    position += (uint)pointRecordLength;
                }
            }

            // Cancel button was pressed.
            else if (dr == System.Windows.Forms.DialogResult.Cancel) {
                return Result.Cancel;
            } else {
                return Result.Failure;
            }






            RhinoApp.WriteLine(lasf);
            RhinoApp.WriteLine("pointStart = "+ pointStart.ToString());
            RhinoApp.WriteLine("pointCount = " + pointCount.ToString());

            //#region beginScript




            ////Transform xform0 = RhinoDocument.EarthAnchorPoint.GetModelToEarthTransform(RhinoDocument.ModelUnitSystem);
            ////   Transform latLng;
            ////   xform0.TryGetInverse(out latLng);



            //string[] stringRow = System.IO.File.ReadAllLines(txt);
            //Point4d[] points = new Point4d[stringRow.Length];
            //for (int i = 0; i < stringRow.Length; i++) {
            //    string[] xyz = stringRow[i].Split(' ');
            //    if (xyz.Length != 4) {
            //        continue;
            //    }
            //    double x = double.Parse(xyz[0], System.Globalization.NumberStyles.Float);
            //    double y = double.Parse(xyz[1], System.Globalization.NumberStyles.Float);
            //    double z = double.Parse(xyz[2], System.Globalization.NumberStyles.Float);
            //    double a = double.Parse(xyz[3], System.Globalization.NumberStyles.Float);


            //    Point4d pt = new Point4d(x, y, z, a);
            //    //pt.Transform(latLng);
            //    points[i] = pt;
            //}



            ////   points.Sort(delegate (Point3d pt1, Point3d pt2) { return pt2.X.CompareTo(pt1.X); });
            ////    points.Sort(delegate (Point3d pt1, Point3d pt2) { return pt2.Y.CompareTo(pt1.Y); });
            ////







            ////convert point3d to node2
            ////grasshopper requires that nodes are saved within a Node2List for Delaunay
            //var nodes = new Grasshopper.Kernel.Geometry.Node2List();
            //for (int i = 0; i < points.Length; i++) {
            //    //notice how we only read in the X and Y coordinates
            //    //  this is why points should be mapped onto the XY plane
            //    nodes.Append(new Grasshopper.Kernel.Geometry.Node2(points[i].X, points[i].Y));
            //}

            ////solve Delaunay
            //Mesh delMesh = new Mesh();
            //List<Grasshopper.Kernel.Geometry.Delaunay.Face> faces = new List<Grasshopper.Kernel.Geometry.Delaunay.Face>();
            //faces = Grasshopper.Kernel.Geometry.Delaunay.Solver.Solve_Faces(nodes, 1);

            ////output
            //delMesh = Grasshopper.Kernel.Geometry.Delaunay.Solver.Solve_Mesh(nodes, 1, ref faces);

            //for (int i = 0; i < delMesh.Vertices.Count; i++) {
            //    delMesh.Vertices[i] = new Point3f(delMesh.Vertices[i].X, delMesh.Vertices[i].Y, (float)points[i].Z);
            //}

            //delMesh.VertexColors.CreateMonotoneMesh(System.Drawing.Color.White);
            //for (int i = 0; i < delMesh.VertexColors.Count; i++) {
            //    delMesh.VertexColors[i] = Color.FromArgb(255, (int)points[i].W, (int)points[i].W, (int)points[i].W);
            //}



            ////for (int i = 0; i < points.Count; i++) {
            ////    Ray3d ray = new Ray3d(new Point3d(points[i].X, points[i].Y, double.MinValue),Vector3d.ZAxis);
            ////    int[] index;
            ////    Rhino.Geometry.Intersect.Intersection.MeshRay(delMesh, ray, out index);
            ////    delMesh.Vertices[index[0]] = new Point3f((float)points[i].X, (float)points[i].Y, (float)points[i].Z);
            ////}


            //A = delMesh;
            //#endregion

            Point3d[] updatePoints = new Point3d[points.Length];
            for (int i = 0; i < points.Length; i++) {
                updatePoints[i] = new Point3d(points[i].X, points[i].Y, points[i].Z);
            }
            RhinoDoc.ActiveDoc.Objects.AddPoints(updatePoints);
            RhinoApp.WriteLine("Las file imported 2");

            // ---

            return Result.Success;
        }
    }
}
