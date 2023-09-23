if (m_player == noone)
{
	m_player = instance_create_layer(80, 38, "Objects", obj_player);
	
	with (m_player)
	{
		m_nn = NN_Create();
		
		if (m_nn < 0)
		{
			assert("ERROR - NN_Create()");
		}
	}
}
else
{
	var nnID = -1;
	
	with (m_player)
	{
		if (m_is_dead)
		{
			nnID = m_nn;
			instance_destroy();
		}
	}
	
	if (nnID != -1)
	{
		m_population[m_population_cursor] = nnID;
		m_population_cursor++;
		m_player = noone;
	}
	
	if (m_population_cursor == m_population_size)
	{
		for (var _i = 0; _i < m_selection_size; _i++)
		{
			var _index_minimum = population_get_min_index();
			m_selection_size[_i] = m_population[_index_minimum];
			m_population[_index_minimum] = -1;
		}
		
		for (var _i = 0; _i < m_population_size; _i++)
		{
			NN_Destroy(m_population[_i]);
		}
		
		m_population_cursor = 0;
	}
}
