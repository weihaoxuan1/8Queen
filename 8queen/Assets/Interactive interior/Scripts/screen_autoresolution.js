var xres;
var yres;

var full_screen=false;

xres = Screen.currentResolution.width;

yres = Screen.currentResolution.height;

    Application.targetFrameRate = -1;

function Update() {

if (full_screen==false){
	Screen.SetResolution (xres, yres, true);
	full_screen=true;
}

}




     
