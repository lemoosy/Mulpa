function assert(_message)
{
	show_debug_message(_message);
	game_end();
}

function world_to_string()
{
	/// Création de la matrice.
	
	var _w = (room_width / global.tile_size_x);
	var _h = (room_height / global.tile_size_y);
	
	var _matrix = array_create(_h);
	
	for (var _j = 0; _j < _h; _j++)
	{
		_matrix[_j] = array_create(_w);
		
		for (var _i = 0; _i < _w; _i++)
		{
			_matrix[_j][_i] = "O";
		}
	}
	
	/// Initialisation des valeurs de la matrice.
	
	with (all)
	{
		var _i = (x + (bbox_right - bbox_left) / 2) / global.tile_size_x;
		var _j = (y + (bbox_bottom - bbox_top) / 2) / global.tile_size_y;
		
		_i = floor(_i);
		_j = floor(_j);
		
		if ((0 <= _i && _i < _w) && (0 <= _j && _j < _h))
		{
			switch (object_index)
			{
				case obj_player:
				
					_matrix[_j][_i] = "P";
				
				break;
				
				case obj_exit:
				
					if (_matrix[_j][_i] != "P")
					{
						_matrix[_j][_i] = "E";
					}
				
				break;
				
				case obj_monster:
				
					if (_matrix[_j][_i] != "P" && _matrix[_j][_i] != "E")
					{
						_matrix[_j][_i] = "M";
					}
				
				break;
				
				case obj_wall:
				
					if (_matrix[_j][_i] != "P" && _matrix[_j][_i] != "E" && _matrix[_j][_i] != "M")
					{
						_matrix[_j][_i] = "W";
					}
					
				break;
			}
		}
	}
	
	/// Conversion de la matrice en string.
	
	//var _string = "";
	
	//for (var _j = 0; _j < _h; _j++)
	//{
	//	for (var _i = 0; _i < _w; _i++)
	//	{
	//		_string += _matrix[_j][_i] + " ";
	//	}
		
	//	_string += "\n";
	//}
	
	//show_debug_message(_string);
	//show_debug_message("--------------------------");
	
	var _string = string(_w) + "x" + string(_h) + ":";
	
	for (var _j = 0; _j < _h; _j++)
	{
		for (var _i = 0; _i < _w; _i++)
		{
			_string += _matrix[_j][_i];
		}
	}
	
	return _string;
}
