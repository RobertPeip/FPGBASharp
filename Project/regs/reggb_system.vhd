library IEEE;
use IEEE.std_logic_1164.all;  
use IEEE.numeric_std.all;     

use work.pProc_bus_gb.all;
use work.pRegmap.all;

package pReg_gb_system is

   -- range 0x200 .. 0x800
   --   (                             adr      upper    lower    size   default   accesstype)                                     
   constant IE      : regmap_type := (12#200#,  15,      0,        1,        0,   readwrite); -- Interrupt Enable Register
   constant IF      : regmap_type := (12#200#,  31,     16,        1,        0,   readwrite); -- Interrupt Request Flags / IRQ Acknowledge  
   
   constant WAITCNT : regmap_type := (12#204#,  15,      0,        1,        0,   readwrite); -- Game Pak Waitstate Control  

   constant IME     : regmap_type := (12#208#,  31,      0,        1,        0,   readwrite); -- Interrupt Master Enable Register  
   
   constant POSTFLG : regmap_type := (12#300#,   7,      0,        1,        0,   readwrite); -- Undocumented - Post Boot Flag  
   constant HALTCNT : regmap_type := (12#300#,  15,      8,        1,        0,   writeonly); -- Undocumented - Power Down Control

 --constant ?       : regmap_type := (12#410#,  15,      0,        1,        0,   readwrite); -- Undocumented - Purpose Unknown / Bug ??? 0FFh  
 
   constant MemCtrl : regmap_type := (12#800#,  31,      0,        1,        0,   readwrite); -- Undocumented - Internal Memory Control (R/W) -- Mirrors of 4000800h (repeated each 64K) 
      
   
end package;