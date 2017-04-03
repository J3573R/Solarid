Shader "Sprites/Cooldown"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Progress("Bar Progress", Float) = 0
	}

		SubShader
	{

		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile DUMMY PIXELSNAP_ON
		#include "UnityCG.cginc"

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};
	float _Progress;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.color = IN.color;
		OUT.texcoord = IN.texcoord;

		#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
		#endif

		return OUT;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f IN) : SV_Target
	{	

		fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
		fixed4 black = { 0, 0, 0, 0 };
		c.rgb *= c.a;

		float2 pos = { 0.5 - IN.texcoord.x, 0.5 - IN.texcoord.y};
		pos = normalize(pos);

		float2 up = { 1, 0 };
		float angle = acos(dot(pos, up));
		float prog = _Progress * 2 * 3.14159265;

		if (IN.texcoord.y > 0.5)
		{
			if (angle > prog)
			{
				c.rgb = lerp(c, black, 0.75);
			}
		}
		else
		{
			if (angle < (2 * 3.14159265) - prog)
			{
				c.rgb = lerp(c, black, 0.75);
			}
		}

		return c;
	}

	ENDCG
	}
	}
}