function vec2_set(_x, _y)
{
	return [_x, _y];
}

function vec2_add(_v1, _v2)
{
	return vec2_set(_v1[0] + _v2[0], _v1[1] + _v2[1]);
}

function vec2_sub(_v1, _v2)
{
	return vec2_set(_v1[0] - _v2[0], _v1[1] - _v2[1]);
}

function vec2_scale(_v, _s)
{
	return vec2_set(_v[0] * _s, _v[1] * _s);
}

function vec2_distance(_v1, _v2)
{
	return sqrt(power(_v1[0] - _v2[0], 2) + power(_v1[1] - _v2[1], 2));
}
