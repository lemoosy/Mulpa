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
			phy_linear_velocity_x = -m_speed[0];
		}
		else
		{
			phy_linear_velocity_x = +m_speed[0];
		}
	}
	else
	{
		phy_linear_velocity_x = 0;
	}
	
	if (m_key_jump and m_on_ground)
	{
		phy_linear_velocity_y = -m_speed[1];
	}
	
	if ((bbox_left < 0) or (bbox_right > room_width))
	{
		phy_linear_velocity_x = 0;
	}
}

function player_update_position()
{
	if (bbox_left < 0)
	{
		phy_position_x -= bbox_left;
	}
	
	if (bbox_right > room_width)
	{
		phy_position_x -= (bbox_right - room_width);
	}
	
}

function player_update_state()
{
	m_on_ground = false;
}

function player_update_distance_exit()
{
	m_distance_exit = 0;
}
