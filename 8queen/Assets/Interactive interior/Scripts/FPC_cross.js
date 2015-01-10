var cross: Texture;

 Screen.showCursor = false;

function OnGUI () { 

GUI.DrawTexture( Rect (Screen.width/2-cross.width/2,Screen.height/2-cross.height/2, cross.width, cross.height),cross);

}