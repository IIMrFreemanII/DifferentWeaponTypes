using System;
using UnityEngine;

public class AutoTiling : MonoBehaviour
{
	private Renderer _renderer;
    void Start ()
    	{
	        float scaleX = transform.localScale.x * 2;
	        float scaleY = transform.localScale.y * 2;
	        float scaleZ = transform.localScale.z * 2;

	        Renderer renderer = GetComponent<Renderer>();
	        
	        if (Math.Abs(transform.localScale.x - 1) < 1 && Math.Abs(transform.localScale.y - 1) < 1 && Math.Abs(transform.localScale.z - 1) < 1)
	        {
		        renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
	        } 
	        if (Math.Abs(transform.localScale.x - 1) < 1)
	        {
		        renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(scaleY, scaleZ));
	        } 
	        if (Math.Abs(transform.localScale.y - 1) < 1)
	        {
		        renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(scaleX, scaleZ));
	        } 
	        if (Math.Abs(transform.localScale.z - 1) < 1)
	        {
		        renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
	        }
        }
}
