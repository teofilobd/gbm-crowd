#define PI 3.14159265358979323846264
#define PI_2 PI/2.0

fixed4 _ObstacleVelocity;
fixed _ObstacleID;
fixed4 _AgentPosition;
fixed4 _AgentVelocity;

bool isinf(float s)
{
  // By IEEE 754 rule, 2*Inf equals Inf
  return (2*s == s) && (s != 0);
}
 
bool isnan(float s)
{
  // By IEEE 754 rule, NaN is not equal to NaN
  return s != s;
}

fixed4 getColorMap(fixed v, fixed vmin, fixed vmax)
{
   fixed4 c = fixed4(1.0, 1.0, 1.0, 1.0);
   fixed dv;

   if (v < vmin)
      v = vmin;
   if (v > vmax)
      v = vmax;
   dv = vmax - vmin;

   if (v < (vmin + 0.25 * dv)) 
   {
      c.r = 0;
      c.g = 4 * (v - vmin) / dv;
   } else if (v < (vmin + 0.5 * dv)) 
   {
      c.r = 0;
      c.b = 1 + 4 * (vmin + 0.25 * dv - v) / dv;
   } else if (v < (vmin + 0.75 * dv)) 
   {
      c.r = 4 * (v - vmin - 0.5 * dv) / dv;
      c.b = 0;
   } else 
   {
      c.g = 1 + 4 * (vmin + 0.75 * dv - v) / dv;
      c.b = 0;
   }

   return c;
}

fixed dot2(fixed2 v1, fixed2 v2)
{
    return v1.x*v2.x + v1.y*v2.y;
}

fixed cross2(fixed2 v1, fixed2 v2)
{
    return v1.x*v2.y - v2.x*v1.y;
}

fixed signedAngle(fixed2 v1, fixed2 v2)
{
    return atan2(cross2(v1, v2), dot2(v1, v2));
}

float fixAngleValue(float angle)
{
    if (abs(angle) > PI )
        return angle - sign(angle) * 2.0 * PI;
    
    return angle;
}