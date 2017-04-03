Shader "Sprites/CastBar"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_ColorCastLeft("Bar color", Color) = (1, 1, 1, 1)
		_ColorSegmentTwo("Segment 2 progress color", Color) = (1, 1, 1, 1)
		_ColorCastElapsed("Bar progress color", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Progress("Bar Progress", Float) = 0
		_SegmentTwo("Second Segment", Float) = 0
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

			fixed4 _ColorCastLeft;
			fixed4 _ColorSegmentTwo;
			fixed4 _ColorCastElapsed;
			float _Progress;
			float _SegmentTwo;

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
				c.rgb *= c.a;

				if (IN.texcoord.x > _Progress + _SegmentTwo)
				{
					c.rgb = _ColorCastElapsed.rgb;
				}
				else if (IN.texcoord.x > _Progress)
				{
					c.rgb = _ColorSegmentTwo.rgb;
				}
				else
				{
					c.rgb = _ColorCastLeft.rgb;
				}

				return c;
			}
			ENDCG
		}
	}
}