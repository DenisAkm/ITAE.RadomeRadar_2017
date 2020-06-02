using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;

namespace Apparat
{
    public abstract class Renderable
    {
      public Matrix transform = Matrix.Identity;
      public abstract void Render();
      public abstract void Dispose();

      public abstract Matrix Transform
      {
        get;
        set;
      }
    }
}
