/// Initialise les DLL depuis le basecode 'Basecode_DLL'.
DLL_Init();

/// Active le mode IA ou le mode Solo.
global.ai = true;

global.time_step = (1 / room_speed);

global.tile_size_x = 16;
global.tile_size_y = 16;
