function player_update_input()
{
	m_key_left = keyboard_check(vk_left);
	m_key_right = keyboard_check(vk_right);
	m_key_jump = keyboard_check(vk_up);
}

function player_update_velocity()
{
	if (m_key_left xor m_key_right)
	{
		if (m_key_left)
		{
			phy_linear_velocity_x = -m_speed[0]
		}
		else
		{
		}
	}
	else
	{
		phy_linear_velocity_x = 0;
	}
}

function player_update_position()
{
	
}