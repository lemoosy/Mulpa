var _b = round(bbox_bottom) == round(phy_collision_y[0]);
_b = _b && round(bbox_bottom) == round(phy_collision_y[1]);

if ((m_on_ground == false) and _b)
{
	m_on_ground = true;
}
