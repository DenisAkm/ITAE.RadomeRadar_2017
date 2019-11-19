// Gmsh project created on Wed Dec 27 13:28:54 2017
SetFactory("OpenCASCADE");
//+
Sphere(1) = {0, 0, 0, 0.5, 0, Pi/2, 2*Pi};
//+
Physical Surface("Radome") = {1};
//+
Physical Surface("Radome") += {1};
//+
Transfinite Surface {1};
//+
Compound Surface(3) = {1};
//+
Physical Surface("Buttom") = {2};
//+
Show "*";
//+
Hide {
Surface{2,3};
Volume{1};
}

