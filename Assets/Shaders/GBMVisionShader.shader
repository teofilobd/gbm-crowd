Shader "Custom/GBMVisionShader" 
{
	Properties 
	{
		_ObstacleVelocity ("ObstacleVelocity", Vector) = (0,0,0,0)
		_ObstacleID ("ObstacleID", Range(0,100)) = 1
		_DistanceToGoal ("DistanceToGoal", Range(0,100)) = 1
		_AgentPosition ("AgentPosition", Vector) = (0,0,0,0)
		_AgentVelocity ("AgentVelocity", Vector) = (0,0,0,0)
		_SigmaDca ("SigmaDca", Range(0,100)) = 0.3
		_SigmaTtca ("SigmaTtca", Range(0,100)) = 2.0
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
		
		#define EXP_NEG_0_5 0.6065306597126334f

		fixed _DistanceToGoal;
		fixed _SigmaDca;
		fixed _SigmaTtca;
		
		struct v2f 
		{
                float4 pos : SV_POSITION;
                fixed2 relativePosition : TEXCOORD0;
                fixed2 relativeVelocity : TEXCOORD1;
		};

		v2f vert (appdata_base v)
        {
        	v2f o;
        	
        	o.relativePosition = mul(unity_ObjectToWorld, v.vertex).xz - _AgentPosition.xz;
        	o.relativeVelocity = _ObstacleVelocity.xz - _AgentVelocity.xz;
        	o.pos = UnityObjectToClipPos (v.vertex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
        	fixed relativeSpeed    = length(i.relativeVelocity);
        	fixed relativeDistance = length(i.relativePosition);
        	
        	if(relativeSpeed == 0.0)
        		relativeSpeed = 1e-6f;
        		
        	fixed relativeSpeed2    = pow(relativeSpeed, 2.0);
			fixed2 agentVelocity    = _AgentVelocity.xz;
			fixed2 agentVelocityDir = normalize(agentVelocity);
			 
		    //*****************************
		    //* Initialize the variables
		    //*****************************
		    fixed ttca = 0.0;
		    fixed dca  = 0.0;
		    fixed dTtcadV = 0.0;
		    fixed dTtcadT = 0.0;
		    fixed dDcadV = 0.0;
		    fixed dDcadT = 0.0;
		    fixed C = 0.0,
		          invSigmaTtca2 = 1.0/pow(_SigmaTtca,2.0), 
		          invSigmaDca2  = 1.0/pow(_SigmaDca,2.0); 
		    fixed dCdV = 0.0;
		    fixed dCdT = 0.0;

			fixed invRelativeSpeed2 = 1.0f/(relativeSpeed2);

		    //*****************************
		    //*  TTCA and DCA computation
		    //*****************************
		    ttca = - dot(i.relativePosition, i.relativeVelocity) * invRelativeSpeed2;
		    fixed2 vecDca = i.relativePosition + i.relativeVelocity * ttca;
		    dca  = length(vecDca);

		    //*****************************
		    //*  Total cost computation
		    //*****************************
		    if(!isnan(ttca) && !isinf(ttca) && ttca >= 0.0f && ttca < 10.0f) 
		    {
		        fixed ttcaAux = invSigmaTtca2 * ttca;
		        fixed dcaAux  = invSigmaDca2 * dca;
		        C = pow(EXP_NEG_0_5, (ttcaAux * ttca + dcaAux * dca));

		        //*****************************
		        //*  TTCA and DCA derivatives
		        //*****************************
		        fixed2 vecAux = i.relativePosition + 2 * ttca * i.relativeVelocity;

		        if(dca==0.0f)
		            dca=1e-6f;
		        float invDca = 1.0f/dca;

		        dTtcadV =   dot(vecAux, agentVelocityDir) * invRelativeSpeed2;
		        dTtcadT = - dot(vecAux, fixed2(agentVelocity.y, -agentVelocity.x)) * invRelativeSpeed2;
		        dDcadV  =   dot(vecDca, dTtcadV * i.relativeVelocity - ttca * agentVelocityDir) * invDca;
		        dDcadT  =   dot(vecDca, dTtcadT * i.relativeVelocity + ttca * fixed2(agentVelocity.y, -agentVelocity.x)) * invDca;

		        //*****************************
		        //*  Total cost derivatives
		        //*****************************
		        dCdV = -C * (dTtcadV * ttcaAux + dDcadV * dcaAux);
		        dCdT = -C * (dTtcadT * ttcaAux + dDcadT * dcaAux);
		    }



		    if((dCdV == 0.0f && dCdT == 0.0f) || isnan(dCdV+dCdT) || isinf(dCdV+dCdT) ||
		        length(i.relativePosition) > _DistanceToGoal)
		    {
		        dCdV = dCdT = 0.0f;
		    }
		    else
		    {
		        fixed moffset = 10.0f;

		        if (_ObstacleID >= 0)
		        {
		                dCdV += moffset;
		                dCdT += moffset;
		        } else
		        {
		                dCdV -= moffset;
		                dCdT -= moffset;
		        }
		    }

			return fixed4 (dCdV,dCdT, 1.0, 1.0);
        }

		ENDCG
		}
	} 
}
