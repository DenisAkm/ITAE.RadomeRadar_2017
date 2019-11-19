// Gmsh project created on Fri Dec 29 14:56:15 2017
SetFactory("OpenCASCADE");
//+
Sphere(1) = {0, 0, 0, 9364, 0, Pi/2, 2*Pi};
//+
Translate {0, 0, -1459} {
  Surface{1}; Surface{2}; 
}
