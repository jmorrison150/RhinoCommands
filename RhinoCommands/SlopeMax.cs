using System;
using Rhino;
using Rhino.Commands;

namespace RhinoCommands {
    public class SlopeMax : Command {
        static SlopeMax _instance;
        public SlopeMax() {
            _instance = this;
        }

        ///<summary>The only instance of the SlopeMax command.</summary>
        public static SlopeMax Instance {
            get; private set;
        }

        public override string EnglishName {
            get { return "SlopeMax"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode) {
            // TODO: complete command.
            RhinoApp.WriteLine("Slope = Max");

            return Result.Success;
        }
    }
}