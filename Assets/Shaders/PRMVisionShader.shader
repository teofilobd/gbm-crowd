Shader "Custom/PRMVisionShader" 
{
	Properties 
	{
		_ObstacleVelocity ("ObstacleVelocity", Vector) = (0,0,0,0)
		_ObstacleID ("ObstacleID", Range(0,100)) = 1
		_AgentPosition ("AgentPosition", Vector) = (0,0,0,0)
		_AgentVelocity ("AgentVelocity", Vector) = (0,0,0,0)
		_GoalVelocity ("GoalVelocity", Vector) = (0,0,0,0)
	}

	SubShader 
	{
		Tags { "RenderType" = "Opaque"}
		Pass 
		{
		
		CGPROGRAM
		
		#pragma target 3.0
		
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "VisionShaderDefs.cginc"
		
		half4 _GoalVelocity;
		fixed _A;
		fixed _B;
		fixed _C;
		half _Ttg;
		
		struct v2f 
		{
            float4 pos : SV_POSITION;
            half depth : TEXCOORD0;
            half speed : TEXCOORD1;
            half speed_Alpha : TEXCOORD2;
            half speed_Alpha_Goal : TEXCOORD3;
            half alpha : TEXCOORD4;
            half alphaDot : TEXCOORD5;
        };

		v2f vert (appdata_base v)
        {
        	v2f o;
        	
        	half2 relativeVelocity     = _ObstacleVelocity.xz - _AgentVelocity.xz;
            half2 relativeVelocityGoal = _ObstacleVelocity.xz - _GoalVelocity.xz;        	
        	half2 relativePosition = mul(unity_ObjectToWorld, v.vertex).xz - _AgentPosition.xz;
	        
	        // Depth (to compute ttc)
	        o.depth       = length(relativePosition);
	        fixed2 obstacle_Dir  = normalize(relativePosition);
	        
	        o.speed = length(_ObstacleVelocity.xz);
	        
	        // SpeedAlpha (to compute ttc)
	        o.speed_Alpha      = dot(relativeVelocity,     -obstacle_Dir);
	        o.speed_Alpha_Goal = dot(relativeVelocityGoal, -obstacle_Dir);
	        fixed2 obstacle_Dir_Next    = normalize(relativePosition + relativeVelocity);
	        
	        fixed2 orientationv = normalize(_AgentVelocity.xz);
	        half alpha       = -signedAngle(orientationv, obstacle_Dir);
	        half alpha_Next  = -signedAngle(orientationv, obstacle_Dir_Next);
	        
	        // Ensure -PI < alpha < PI
	        o.alpha = fixAngleValue(alpha);
	        
	        // alphaDot = alpha(t+1s) - alpha(t)
	        o.alphaDot = alpha_Next - alpha;
	        
	        // Ensure -PI < alpha_Dot < PI
	        o.alphaDot = fixAngleValue(o.alphaDot);
        	
        	o.pos = UnityObjectToClipPos (v.vertex);
            return o;
        }
        
        fixed4 frag (v2f i) : SV_Target
        {
        	// asymetry for agent passing first and second
		    fixed3 params[2];
			params[0].x = _A;
			params[0].y = _B;
			params[0].z = _C;
			
			params[1].x = _A;
			params[1].y = _B + 0.1;
			params[1].z = _C;
			
			// initial return values if there is no collision
			fixed first = 1.0;
			half ttc = 1.0e30f;
			half2 theta_Dot = half2(0.0, 0.0);
			
		    fixed threshold  = PI_2;
		    half ttcg = 0.0f;

			fixed agentRadius = 0.3; // TODO: Fix this value.
		    half dist = max(0.0, i.depth - 1.5 * agentRadius);

		    if(dist > 0.0f)
			{
				if(i.speed_Alpha > 0.0f)
					ttc   = dist / i.speed_Alpha;
				
				if(i.speed_Alpha_Goal > 0.0f)				
					ttcg  = dist / i.speed_Alpha_Goal;
			}
			else
			{
				ttc = 0.0f;
			}

			if(ttc >= 0.0 && ttc < 10.0) 
			{			
				half alpha_Dot = i.alphaDot;

				// if positive, go first
				first = i.alpha * alpha_Dot;
			
				if(ttc < 1 && abs(alpha_Dot) > PI_2)
				{
					if(alpha_Dot > 0.0)
						alpha_Dot = PI - alpha_Dot;
					else
						alpha_Dot = -PI - alpha_Dot;
				}

				int paramIndex = first < 0.0 ? 1 : 0;
				half v = params[paramIndex].x + params[paramIndex].y / pow(ttc, params[paramIndex].z);
				v  = clamp(v, 0.0, threshold);
				
				if (ttc > 1.0 && ttcg >= 10.0)
				{
					// ignore: collision and go to goal (using velocity in a direction of goal is ok)
					// Reset ttc value so this pixel is not used
					ttc = 1.0e30f;
				}
				else if ( abs(alpha_Dot) < v && (_Ttg >= ttc || ttc < 1.0f) )
				{
					// Only this is a future collision, compute turning angles
					theta_Dot = fixed2(clamp( (- v + alpha_Dot), -threshold, threshold),
									   clamp( (  v + alpha_Dot), -threshold, threshold));
				}
				else				
				{
					// Reset ttc value so this pixel is not used
					ttc = 1.0e30f;
				}
			}
			return fixed4 (theta_Dot.x, theta_Dot.y, ttc, first);
        }

		ENDCG
		}
	} 
}
