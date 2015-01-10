var Speed = 0.5; 

function Update () 

{

var offset = Time.time * Speed;

renderer.material.SetTextureOffset ("_MainTex", Vector2(0,offset));
renderer.material.SetTextureOffset ("_BumpMap", Vector2(0,offset));

}


