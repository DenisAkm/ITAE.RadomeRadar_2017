// Gmsh project created on Wed Dec 27 14:19:20 2017
SetFactory("OpenCASCADE");
//+
Circle(1) = {0, 0, 0, 100, 0, 2*Pi};
//+
Disk(1) = {0, 0, 0, 100, 100};
