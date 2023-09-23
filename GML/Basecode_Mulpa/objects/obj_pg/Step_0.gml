if (m_player == noone)
{
	m_player = instance_create_layer(16, 96, "Objects", obj_player);
	
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
			NN_SetScore(m_nn, ShortestPath_Get(world_to_string()));
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
		m_gen++;
		show_debug_message("Generation : " + string(m_gen));
		
		for (var _i = 0; _i < m_selection_size / 2; _i++)
		{
			var _index_minimum = population_get_min_index();
			m_selection[_i] = m_population[_index_minimum];
			m_population[_index_minimum] = -1;
		}
		
		
		for (var _i = 0; _i < m_population_size; _i++)
		{
			NN_Destroy(m_population[_i]);
		}
		
		for (var _i = m_selection_size / 2; _i < m_selection_size; _i++)
		{
			var r1 = random_range(0, (m_selection_size / 2) - 1);
			var r2 = random_range(0, (m_selection_size / 2) - 1);
			
			if (r1 == r2)
			{
				continue;
			}
			
			var nn1 = m_selection[r1];
			var nn2 = m_selection[r2];
			
			var nnRes = NN_Crossover(nn1, nn2);
			
			m_selection[_i] = nnRes;
		}
		
		for (var _i = 0; _i < m_selection_size; _i++)
		{
			m_population[_i] = m_selection[_i];
			m_selection[_i] = -1;
		}
		
		m_population_cursor = m_selection_size;
	}
}
