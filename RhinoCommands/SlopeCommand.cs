using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace RhinoCommands {
    public class SlopeCommand : Command {
        public SlopeCommand() {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static SlopeCommand Instance {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName {
            get { return "Slope"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode) {
            // TODO: start here modifying the behaviour of your command.
            // ---
            RhinoApp.WriteLine("The {0} command will calculate very basic overall slope of a curve.", EnglishName);

            //GetInteger
            //GetLine
            //GetNumber
            //GetObject
            //GetPoint
            //GetString
            //GetTransform


           

            const Rhino.DocObjects.ObjectType geometryFilter = Rhino.DocObjects.ObjectType.Curve;
            GetObject getCurve = new GetObject();
            getCurve.SetCommandPrompt("selece a curve");
            getCurve.GeometryFilter = geometryFilter;
            getCurve.GroupSelect = false;
            getCurve.SubObjectSelect = false;
            getCurve.EnableClearObjectsOnEntry(false);
            getCurve.DeselectAllBeforePostSelect = false;

            GetResult res = getCurve.Get();
            Rhino.DocObjects.RhinoObject rhinoObject = getCurve.Object(0).Object();
            Curve c = (Curve) rhinoObject.Geometry;

            double rise = c.PointAtEnd.Z - c.PointAtStart.Z;
            double run = c.GetLength();
            if (run==0) { return Result.Nothing; }
            double slope = rise / run;

            RhinoApp.WriteLine("Slope = " + slope.ToString());

            // ---

            return Result.Success;
        }
    }
}
