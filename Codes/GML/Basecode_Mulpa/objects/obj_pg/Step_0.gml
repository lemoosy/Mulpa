
/// Si aucune IA joue, on crée une IA.
if (m_player == noone)
{
	m_player = instance_create_layer(16, 96, "Objects", obj_player);
	
	var _nn_id = -1;
	
	if (!children_is_empty())
	{
		_nn_id = children_pop();		
	}
	else
	{
		_nn_id = NN_Create();
	}
	
	with (m_player)
	{
		m_nn = _nn_id;

		if (m_nn < 0)
		{
			assert("ERROR - NN_Create()");
		}
	}
}

/// Si l'IA joue.
else
{
	var _nn_id = -1;
	
	with (m_player)
	{
		if (m_is_dead)
		{
			_nn_id = m_nn;
			NN_UpdateScore(_nn_id, world_to_string());
			//show_debug_message("PCC : " + string(NN_GetScore(_nn_id)));
			instance_destroy();
		}
	}
	
	/// Si le joueur est mort, on l'ajoute à la population.
	if (_nn_id != -1)
	{
		population_insert(_nn_id);
		m_player = noone;
	}
	
	/// Si la population est pleine.
	if (population_is_full())
	{
		m_gen++;
		show_debug_message("Generation >> " + string(m_gen));
		
		for (var _i = 0; _i < m_selection_size; _i++)
		{
			var _nn_id_minimum = population_remove_minimum();
			if (_i < 10) show_debug_message(NN_GetScore(_nn_id_minimum));
			selection_insert(_nn_id_minimum);
		}
		
		population_destroy();
		
		for (var _i = 0; _i < m_children_size; _i++)
		{
			var _r1 = random_range(0, (m_selection_size - 1));
			var _r2 = random_range(0, (m_selection_size - 1));
			
			if (_r1 == _r2)
			{
				continue;
			}
			
			var _nn_id_1 = m_selection[_r1];
			var _nn_id_2 = m_selection[_r2];
			
			var _nn_id_res = NN_Crossover(_nn_id_1, _nn_id_2);
			
			children_insert(_nn_id_res);
		}
		
		selection_to_population();
	}
}
