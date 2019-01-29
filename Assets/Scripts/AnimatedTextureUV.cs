using UnityEngine;
using System.Collections;

public class AnimatedTextureUV : MonoBehaviour {
	
	public int uvAnimationTileX = 24;
	/// <summary>
	/// Here you can place the number of columns of your sheet.
	/// The above sheet has 24.
	/// </summary>
	
	public int uvAnimationTileY = 1;
	/// <summary>
	/// Here you can place the number of rows of your sheet.
	/// The above sheet has 1.
	/// </summary>
	
	public int framesPerSecond = 10;
	
	// Update is called once per frame
	void Update () {
		//Calculate index
		int index = (int)(Time.time * framesPerSecond);
		
		//Repeat when exposing all frames
		index = index % ( uvAnimationTileX * uvAnimationTileY );
		
		//Size of every tile
		Vector2 size = new Vector2( 1.0f / uvAnimationTileX, 1.0f / uvAnimationTileY );
		
		//Split into horizontal and vertical index
		int uIndex = index % uvAnimationTileX;
		int vIndex = index / uvAnimationTileY;
		
		//Build offset
		//v coordinate is the bottom of the image in opengl so we need to invert
		Vector2 offset = new Vector2( uIndex * size.x, 1.0f - size.y - vIndex * size.y );
		
		renderer.material.SetTextureOffset( "_MainTex", offset );
		renderer.material.SetTextureScale( "_MainTex", size );
	}
}
