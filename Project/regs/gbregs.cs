using System;

public class RegSect_display
{
   /// <summary>
   /// LCD Control 2 R/W
   /// </summary>
   public GBReg DISPCNT;
   /// <summary>
   /// BG Mode (0-5=Video Mode 0-5, 6-7=Prohibited)
   /// </summary>
   public GBReg DISPCNT_BG_Mode;
   /// <summary>
   /// Reserved / CGB Mode (0=GBA, 1=CGB; can be set only by BIOS opcodes)
   /// </summary>
   public GBReg DISPCNT_Reserved_CGB_Mode;
   /// <summary>
   /// Display Frame Select (0-1=Frame 0-1) (for BG Modes 4,5 only)
   /// </summary>
   public GBReg DISPCNT_Display_Frame_Select;
   /// <summary>
   /// H-Blank Interval Free (1=Allow access to OAM during H-Blank)
   /// </summary>
   public GBReg DISPCNT_H_Blank_IntervalFree;
   /// <summary>
   /// OBJ Character VRAM Mapping (0=Two dimensional, 1=One dimensional)
   /// </summary>
   public GBReg DISPCNT_OBJ_Char_VRAM_Map;
   /// <summary>
   /// Forced Blank (1=Allow FAST access to VRAM,Palette,OAM)
   /// </summary>
   public GBReg DISPCNT_Forced_Blank;
   /// <summary>
   /// Screen Display BG0 (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_Screen_Display_BG0;
   /// <summary>
   /// Screen Display BG1 (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_Screen_Display_BG1;
   /// <summary>
   /// Screen Display BG2 (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_Screen_Display_BG2;
   /// <summary>
   /// Screen Display BG3 (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_Screen_Display_BG3;
   /// <summary>
   /// Screen Display OBJ (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_Screen_Display_OBJ;
   /// <summary>
   /// Window 0 Display Flag (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_Window_0_Display_Flag;
   /// <summary>
   /// Window 1 Display Flag (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_Window_1_Display_Flag;
   /// <summary>
   /// OBJ Window Display Flag (0=Off, 1=On)
   /// </summary>
   public GBReg DISPCNT_OBJ_Wnd_Display_Flag;
   /// <summary>
   /// Undocumented - Green Swap 2 R/W
   /// </summary>
   public GBReg GREENSWAP;
   /// <summary>
   /// General LCD Status (STAT,LYC) 2 R/W
   /// </summary>
   public GBReg DISPSTAT;
   /// <summary>
   /// V-Blank flag (Read only) (1=VBlank) (set in line 160..226; not 227)
   /// </summary>
   public GBReg DISPSTAT_V_Blank_flag;
   /// <summary>
   /// H-Blank flag (Read only) (1=HBlank) (toggled in all lines, 0..227)
   /// </summary>
   public GBReg DISPSTAT_H_Blank_flag;
   /// <summary>
   /// V-Counter flag (Read only) (1=Match) (set in selected line) (R)
   /// </summary>
   public GBReg DISPSTAT_V_Counter_flag;
   /// <summary>
   /// V-Blank IRQ Enable (1=Enable) (R/W)
   /// </summary>
   public GBReg DISPSTAT_V_Blank_IRQ_Enable;
   /// <summary>
   /// H-Blank IRQ Enable (1=Enable) (R/W)
   /// </summary>
   public GBReg DISPSTAT_H_Blank_IRQ_Enable;
   /// <summary>
   /// V-Counter IRQ Enable (1=Enable) (R/W)
   /// </summary>
   public GBReg DISPSTAT_V_Counter_IRQ_Enable;
   /// <summary>
   /// V-Count Setting (LYC) (0..227) (R/W)
   /// </summary>
   public GBReg DISPSTAT_V_Count_Setting;
   /// <summary>
   /// Vertical Counter (LY) 2 R
   /// </summary>
   public GBReg VCOUNT;
   /// <summary>
   /// BG0 Control 2 R/W
   /// </summary>
   public GBReg BG0CNT;
   /// <summary>
   /// BG Priority (0-3, 0=Highest)
   /// </summary>
   public GBReg BG0CNT_BG_Priority;
   /// <summary>
   /// Character Base Block (0-3, in units of 16 KBytes) (=BG Tile Data)
   /// </summary>
   public GBReg BG0CNT_Character_Base_Block;
   /// <summary>
   /// 4-5 Not used (must be zero)
   /// </summary>
   public GBReg BG0CNT_UNUSED_4_5;
   /// <summary>
   /// Mosaic (0=Disable, 1=Enable)
   /// </summary>
   public GBReg BG0CNT_Mosaic;
   /// <summary>
   /// Colors/Palettes (0=16/16, 1=256/1)
   /// </summary>
   public GBReg BG0CNT_Colors_Palettes;
   /// <summary>
   /// Screen Base Block (0-31, in units of 2 KBytes) (=BG Map Data)
   /// </summary>
   public GBReg BG0CNT_Screen_Base_Block;
   /// <summary>
   /// Screen Size (0-3)
   /// </summary>
   public GBReg BG0CNT_Screen_Size;
   /// <summary>
   /// BG1 Control 2 R/W
   /// </summary>
   public GBReg BG1CNT;
   /// <summary>
   /// BG Priority (0-3, 0=Highest)
   /// </summary>
   public GBReg BG1CNT_BG_Priority;
   /// <summary>
   /// Character Base Block (0-3, in units of 16 KBytes) (=BG Tile Data)
   /// </summary>
   public GBReg BG1CNT_Character_Base_Block;
   /// <summary>
   /// 4-5 Not used (must be zero)
   /// </summary>
   public GBReg BG1CNT_UNUSED_4_5;
   /// <summary>
   /// Mosaic (0=Disable, 1=Enable)
   /// </summary>
   public GBReg BG1CNT_Mosaic;
   /// <summary>
   /// Colors/Palettes (0=16/16, 1=256/1)
   /// </summary>
   public GBReg BG1CNT_Colors_Palettes;
   /// <summary>
   /// Screen Base Block (0-31, in units of 2 KBytes) (=BG Map Data)
   /// </summary>
   public GBReg BG1CNT_Screen_Base_Block;
   /// <summary>
   /// Screen Size (0-3)
   /// </summary>
   public GBReg BG1CNT_Screen_Size;
   /// <summary>
   /// BG2 Control 2 R/W
   /// </summary>
   public GBReg BG2CNT;
   /// <summary>
   /// BG Priority (0-3, 0=Highest)
   /// </summary>
   public GBReg BG2CNT_BG_Priority;
   /// <summary>
   /// Character Base Block (0-3, in units of 16 KBytes) (=BG Tile Data)
   /// </summary>
   public GBReg BG2CNT_Character_Base_Block;
   /// <summary>
   /// Mosaic (0=Disable, 1=Enable)
   /// </summary>
   public GBReg BG2CNT_Mosaic;
   /// <summary>
   /// Colors/Palettes (0=16/16, 1=256/1)
   /// </summary>
   public GBReg BG2CNT_Colors_Palettes;
   /// <summary>
   /// Screen Base Block (0-31, in units of 2 KBytes) (=BG Map Data)
   /// </summary>
   public GBReg BG2CNT_Screen_Base_Block;
   /// <summary>
   /// Display Area Overflow (0=Transparent, 1=Wraparound; BG2CNT/BG3CNT only)
   /// </summary>
   public GBReg BG2CNT_Display_Area_Overflow;
   /// <summary>
   /// Screen Size (0-3)
   /// </summary>
   public GBReg BG2CNT_Screen_Size;
   /// <summary>
   /// BG3 Control 2 R/W
   /// </summary>
   public GBReg BG3CNT;
   /// <summary>
   /// BG Priority (0-3, 0=Highest)
   /// </summary>
   public GBReg BG3CNT_BG_Priority;
   /// <summary>
   /// Character Base Block (0-3, in units of 16 KBytes) (=BG Tile Data)
   /// </summary>
   public GBReg BG3CNT_Character_Base_Block;
   /// <summary>
   /// Mosaic (0=Disable, 1=Enable)
   /// </summary>
   public GBReg BG3CNT_Mosaic;
   /// <summary>
   /// Colors/Palettes (0=16/16, 1=256/1)
   /// </summary>
   public GBReg BG3CNT_Colors_Palettes;
   /// <summary>
   /// Screen Base Block (0-31, in units of 2 KBytes) (=BG Map Data)
   /// </summary>
   public GBReg BG3CNT_Screen_Base_Block;
   /// <summary>
   /// Display Area Overflow (0=Transparent, 1=Wraparound; BG2CNT/BG3CNT only)
   /// </summary>
   public GBReg BG3CNT_Display_Area_Overflow;
   /// <summary>
   /// Screen Size (0-3)
   /// </summary>
   public GBReg BG3CNT_Screen_Size;
   /// <summary>
   /// BG0 X-Offset 2 W
   /// </summary>
   public GBReg BG0HOFS;
   /// <summary>
   /// BG0 Y-Offset 2 W
   /// </summary>
   public GBReg BG0VOFS;
   /// <summary>
   /// BG1 X-Offset 2 W
   /// </summary>
   public GBReg BG1HOFS;
   /// <summary>
   /// BG1 Y-Offset 2 W
   /// </summary>
   public GBReg BG1VOFS;
   /// <summary>
   /// BG2 X-Offset 2 W
   /// </summary>
   public GBReg BG2HOFS;
   /// <summary>
   /// BG2 Y-Offset 2 W
   /// </summary>
   public GBReg BG2VOFS;
   /// <summary>
   /// BG3 X-Offset 2 W
   /// </summary>
   public GBReg BG3HOFS;
   /// <summary>
   /// BG3 Y-Offset 2 W
   /// </summary>
   public GBReg BG3VOFS;
   /// <summary>
   /// BG2 Rotation/Scaling Parameter A (dx) 2 W
   /// </summary>
   public GBReg BG2RotScaleParDX;
   /// <summary>
   /// BG2 Rotation/Scaling Parameter B (dmx) 2 W
   /// </summary>
   public GBReg BG2RotScaleParDMX;
   /// <summary>
   /// BG2 Rotation/Scaling Parameter C (dy) 2 W
   /// </summary>
   public GBReg BG2RotScaleParDY;
   /// <summary>
   /// BG2 Rotation/Scaling Parameter D (dmy) 2 W
   /// </summary>
   public GBReg BG2RotScaleParDMY;
   /// <summary>
   /// BG2 Reference Point X-Coordinate 4 W
   /// </summary>
   public GBReg BG2RefX;
   /// <summary>
   /// BG2 Reference Point Y-Coordinate 4 W
   /// </summary>
   public GBReg BG2RefY;
   /// <summary>
   /// BG3 Rotation/Scaling Parameter A (dx) 2 W
   /// </summary>
   public GBReg BG3RotScaleParDX;
   /// <summary>
   /// BG3 Rotation/Scaling Parameter B (dmx) 2 W
   /// </summary>
   public GBReg BG3RotScaleParDMX;
   /// <summary>
   /// BG3 Rotation/Scaling Parameter C (dy) 2 W
   /// </summary>
   public GBReg BG3RotScaleParDY;
   /// <summary>
   /// BG3 Rotation/Scaling Parameter D (dmy) 2 W
   /// </summary>
   public GBReg BG3RotScaleParDMY;
   /// <summary>
   /// BG3 Reference Point X-Coordinate 4 W
   /// </summary>
   public GBReg BG3RefX;
   /// <summary>
   /// BG3 Reference Point Y-Coordinate 4 W
   /// </summary>
   public GBReg BG3RefY;
   /// <summary>
   /// Window 0 Horizontal Dimensions 2 W
   /// </summary>
   public GBReg WIN0H;
   /// <summary>
   /// Window 0 Horizontal Dimensions 2 W
   /// </summary>
   public GBReg WIN0H_X2;
   /// <summary>
   /// Window 0 Horizontal Dimensions 2 W
   /// </summary>
   public GBReg WIN0H_X1;
   /// <summary>
   /// Window 1 Horizontal Dimensions 2 W
   /// </summary>
   public GBReg WIN1H;
   /// <summary>
   /// Window 1 Horizontal Dimensions 2 W
   /// </summary>
   public GBReg WIN1H_X2;
   /// <summary>
   /// Window 1 Horizontal Dimensions 2 W
   /// </summary>
   public GBReg WIN1H_X1;
   /// <summary>
   /// Window 0 Vertical Dimensions 2 W
   /// </summary>
   public GBReg WIN0V;
   /// <summary>
   /// Window 0 Vertical Dimensions 2 W
   /// </summary>
   public GBReg WIN0V_Y2;
   /// <summary>
   /// Window 0 Vertical Dimensions 2 W
   /// </summary>
   public GBReg WIN0V_Y1;
   /// <summary>
   /// Window 1 Vertical Dimensions 2 W
   /// </summary>
   public GBReg WIN1V;
   /// <summary>
   /// Window 1 Vertical Dimensions 2 W
   /// </summary>
   public GBReg WIN1V_Y2;
   /// <summary>
   /// Window 1 Vertical Dimensions 2 W
   /// </summary>
   public GBReg WIN1V_Y1;
   /// <summary>
   /// Inside of Window 0 and 1 2 R/W
   /// </summary>
   public GBReg WININ;
   /// <summary>
   /// 0-3 Window_0_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_0_BG0_Enable;
   /// <summary>
   /// 0-3 Window_0_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_0_BG1_Enable;
   /// <summary>
   /// 0-3 Window_0_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_0_BG2_Enable;
   /// <summary>
   /// 0-3 Window_0_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_0_BG3_Enable;
   /// <summary>
   /// 4 Window_0_OBJ_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_0_OBJ_Enable;
   /// <summary>
   /// 5 Window_0_Special_Effect (0=Disable, 1=Enable)
   /// </summary>
   public GBReg WININ_Window_0_Special_Effect;
   /// <summary>
   /// 8-11 Window_1_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_1_BG0_Enable;
   /// <summary>
   /// 8-11 Window_1_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_1_BG1_Enable;
   /// <summary>
   /// 8-11 Window_1_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_1_BG2_Enable;
   /// <summary>
   /// 8-11 Window_1_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_1_BG3_Enable;
   /// <summary>
   /// 12 Window_1_OBJ_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WININ_Window_1_OBJ_Enable;
   /// <summary>
   /// 13 Window_1_Special_Effect (0=Disable, 1=Enable)
   /// </summary>
   public GBReg WININ_Window_1_Special_Effect;
   /// <summary>
   /// Inside of OBJ Window & Outside of Windows 2 R/W
   /// </summary>
   public GBReg WINOUT;
   /// <summary>
   /// 0-3 Outside_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Outside_BG0_Enable;
   /// <summary>
   /// 0-3 Outside_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Outside_BG1_Enable;
   /// <summary>
   /// 0-3 Outside_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Outside_BG2_Enable;
   /// <summary>
   /// 0-3 Outside_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Outside_BG3_Enable;
   /// <summary>
   /// 4 Outside_OBJ_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Outside_OBJ_Enable;
   /// <summary>
   /// 5 Outside_Special_Effect (0=Disable, 1=Enable)
   /// </summary>
   public GBReg WINOUT_Outside_Special_Effect;
   /// <summary>
   /// 8-11 object window_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Objwnd_BG0_Enable;
   /// <summary>
   /// 8-11 object window_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Objwnd_BG1_Enable;
   /// <summary>
   /// 8-11 object window_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Objwnd_BG2_Enable;
   /// <summary>
   /// 8-11 object window_BG0_BG3_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Objwnd_BG3_Enable;
   /// <summary>
   /// 12 object window_OBJ_Enable (0=No Display, 1=Display)
   /// </summary>
   public GBReg WINOUT_Objwnd_OBJ_Enable;
   /// <summary>
   /// 13 object window_Special_Effect (0=Disable, 1=Enable)
   /// </summary>
   public GBReg WINOUT_Objwnd_Special_Effect;
   /// <summary>
   /// Mosaic Size 2 W
   /// </summary>
   public GBReg MOSAIC;
   /// <summary>
   ///  0-3 BG_Mosaic_H_Size (minus 1)
   /// </summary>
   public GBReg MOSAIC_BG_Mosaic_H_Size;
   /// <summary>
   ///  4-7 BG_Mosaic_V_Size (minus 1)
   /// </summary>
   public GBReg MOSAIC_BG_Mosaic_V_Size;
   /// <summary>
   ///  8-11 OBJ_Mosaic_H_Size (minus 1)
   /// </summary>
   public GBReg MOSAIC_OBJ_Mosaic_H_Size;
   /// <summary>
   ///  12-15 OBJ_Mosaic_V_Size (minus 1)
   /// </summary>
   public GBReg MOSAIC_OBJ_Mosaic_V_Size;
   /// <summary>
   /// Color Special Effects Selection 2 R/W
   /// </summary>
   public GBReg BLDCNT;
   /// <summary>
   /// 0 (Background 0)
   /// </summary>
   public GBReg BLDCNT_BG0_1st_Target_Pixel;
   /// <summary>
   /// 1 (Background 1)
   /// </summary>
   public GBReg BLDCNT_BG1_1st_Target_Pixel;
   /// <summary>
   /// 2 (Background 2)
   /// </summary>
   public GBReg BLDCNT_BG2_1st_Target_Pixel;
   /// <summary>
   /// 3 (Background 3)
   /// </summary>
   public GBReg BLDCNT_BG3_1st_Target_Pixel;
   /// <summary>
   /// 4 (Top-most OBJ pixel)
   /// </summary>
   public GBReg BLDCNT_OBJ_1st_Target_Pixel;
   /// <summary>
   /// 5 (Backdrop)
   /// </summary>
   public GBReg BLDCNT_BD_1st_Target_Pixel;
   /// <summary>
   /// 6-7 (0-3, see below) 0 = None (Special effects disabled), 1 = Alpha Blending (1st+2nd Target mixed), 2 = Brightness Increase (1st Target becomes whiter), 3 = Brightness Decrease (1st Target becomes blacker)
   /// </summary>
   public GBReg BLDCNT_Color_Special_Effect;
   /// <summary>
   /// 8 (Background 0)
   /// </summary>
   public GBReg BLDCNT_BG0_2nd_Target_Pixel;
   /// <summary>
   /// 9 (Background 1)
   /// </summary>
   public GBReg BLDCNT_BG1_2nd_Target_Pixel;
   /// <summary>
   /// 10 (Background 2)
   /// </summary>
   public GBReg BLDCNT_BG2_2nd_Target_Pixel;
   /// <summary>
   /// 11 (Background 3)
   /// </summary>
   public GBReg BLDCNT_BG3_2nd_Target_Pixel;
   /// <summary>
   /// 12 (Top-most OBJ pixel)
   /// </summary>
   public GBReg BLDCNT_OBJ_2nd_Target_Pixel;
   /// <summary>
   /// 13 (Backdrop)
   /// </summary>
   public GBReg BLDCNT_BD_2nd_Target_Pixel;
   /// <summary>
   /// Alpha Blending Coefficients 2 W
   /// </summary>
   public GBReg BLDALPHA;
   /// <summary>
   /// 0-4 (1st Target) (0..16 = 0/16..16/16, 17..31=16/16)
   /// </summary>
   public GBReg BLDALPHA_EVA_Coefficient;
   /// <summary>
   /// 8-12 (2nd Target) (0..16 = 0/16..16/16, 17..31=16/16)
   /// </summary>
   public GBReg BLDALPHA_EVB_Coefficient;
   /// <summary>
   /// Brightness (Fade-In/Out) Coefficient 0-4 EVY Coefficient (Brightness) (0..16 = 0/16..16/16, 17..31=16/16
   /// </summary>
   public GBReg BLDY;

   public RegSect_display() 
   {
      DISPCNT = new GBReg(0x000,15,0,1,0x0080,"readwrite");
      DISPCNT_BG_Mode = new GBReg(0x000,2,0,1,0,"readwrite");
      DISPCNT_Reserved_CGB_Mode = new GBReg(0x000,3,3,1,0,"readwrite");
      DISPCNT_Display_Frame_Select = new GBReg(0x000,4,4,1,0,"readwrite");
      DISPCNT_H_Blank_IntervalFree = new GBReg(0x000,5,5,1,0,"readwrite");
      DISPCNT_OBJ_Char_VRAM_Map = new GBReg(0x000,6,6,1,0,"readwrite");
      DISPCNT_Forced_Blank = new GBReg(0x000,7,7,1,0,"readwrite");
      DISPCNT_Screen_Display_BG0 = new GBReg(0x000,8,8,1,0,"readwrite");
      DISPCNT_Screen_Display_BG1 = new GBReg(0x000,9,9,1,0,"readwrite");
      DISPCNT_Screen_Display_BG2 = new GBReg(0x000,10,10,1,0,"readwrite");
      DISPCNT_Screen_Display_BG3 = new GBReg(0x000,11,11,1,0,"readwrite");
      DISPCNT_Screen_Display_OBJ = new GBReg(0x000,12,12,1,0,"readwrite");
      DISPCNT_Window_0_Display_Flag = new GBReg(0x000,13,13,1,0,"readwrite");
      DISPCNT_Window_1_Display_Flag = new GBReg(0x000,14,14,1,0,"readwrite");
      DISPCNT_OBJ_Wnd_Display_Flag = new GBReg(0x000,15,15,1,0,"readwrite");
      GREENSWAP = new GBReg(0x000,31,16,1,0,"readwrite");
      DISPSTAT = new GBReg(0x004,15,0,1,0,"readwrite");
      DISPSTAT_V_Blank_flag = new GBReg(0x004,0,0,1,0,"readonly");
      DISPSTAT_H_Blank_flag = new GBReg(0x004,1,1,1,0,"readonly");
      DISPSTAT_V_Counter_flag = new GBReg(0x004,2,2,1,0,"readonly");
      DISPSTAT_V_Blank_IRQ_Enable = new GBReg(0x004,3,3,1,0,"readwrite");
      DISPSTAT_H_Blank_IRQ_Enable = new GBReg(0x004,4,4,1,0,"readwrite");
      DISPSTAT_V_Counter_IRQ_Enable = new GBReg(0x004,5,5,1,0,"readwrite");
      DISPSTAT_V_Count_Setting = new GBReg(0x004,15,8,1,0,"readwrite");
      VCOUNT = new GBReg(0x004,31,16,1,0,"readwrite");
      BG0CNT = new GBReg(0x008,15,0,1,0,"writeonly");
      BG0CNT_BG_Priority = new GBReg(0x008,1,0,1,0,"readwrite");
      BG0CNT_Character_Base_Block = new GBReg(0x008,3,2,1,0,"readwrite");
      BG0CNT_UNUSED_4_5 = new GBReg(0x008,5,4,1,0,"readwrite");
      BG0CNT_Mosaic = new GBReg(0x008,6,6,1,0,"readwrite");
      BG0CNT_Colors_Palettes = new GBReg(0x008,7,7,1,0,"readwrite");
      BG0CNT_Screen_Base_Block = new GBReg(0x008,12,8,1,0,"readwrite");
      BG0CNT_Screen_Size = new GBReg(0x008,15,14,1,0,"readwrite");
      BG1CNT = new GBReg(0x00A,15,0,1,0,"writeonly");
      BG1CNT_BG_Priority = new GBReg(0x00A,1,0,1,0,"readwrite");
      BG1CNT_Character_Base_Block = new GBReg(0x00A,3,2,1,0,"readwrite");
      BG1CNT_UNUSED_4_5 = new GBReg(0x00A,5,4,1,0,"readwrite");
      BG1CNT_Mosaic = new GBReg(0x00A,6,6,1,0,"readwrite");
      BG1CNT_Colors_Palettes = new GBReg(0x00A,7,7,1,0,"readwrite");
      BG1CNT_Screen_Base_Block = new GBReg(0x00A,12,8,1,0,"readwrite");
      BG1CNT_Screen_Size = new GBReg(0x00A,15,14,1,0,"readwrite");
      BG2CNT = new GBReg(0x00C,15,0,1,0,"readwrite");
      BG2CNT_BG_Priority = new GBReg(0x00C,1,0,1,0,"readwrite");
      BG2CNT_Character_Base_Block = new GBReg(0x00C,3,2,1,0,"readwrite");
      BG2CNT_Mosaic = new GBReg(0x00C,6,6,1,0,"readwrite");
      BG2CNT_Colors_Palettes = new GBReg(0x00C,7,7,1,0,"readwrite");
      BG2CNT_Screen_Base_Block = new GBReg(0x00C,12,8,1,0,"readwrite");
      BG2CNT_Display_Area_Overflow = new GBReg(0x00C,13,13,1,0,"readwrite");
      BG2CNT_Screen_Size = new GBReg(0x00C,15,14,1,0,"readwrite");
      BG3CNT = new GBReg(0x00E,15,0,1,0,"readwrite");
      BG3CNT_BG_Priority = new GBReg(0x00E,1,0,1,0,"readwrite");
      BG3CNT_Character_Base_Block = new GBReg(0x00E,3,2,1,0,"readwrite");
      BG3CNT_Mosaic = new GBReg(0x00E,6,6,1,0,"readwrite");
      BG3CNT_Colors_Palettes = new GBReg(0x00E,7,7,1,0,"readwrite");
      BG3CNT_Screen_Base_Block = new GBReg(0x00E,12,8,1,0,"readwrite");
      BG3CNT_Display_Area_Overflow = new GBReg(0x00E,13,13,1,0,"readwrite");
      BG3CNT_Screen_Size = new GBReg(0x00E,15,14,1,0,"readwrite");
      BG0HOFS = new GBReg(0x010,15,0,1,0,"writeonly");
      BG0VOFS = new GBReg(0x012,15,0,1,0,"writeonly");
      BG1HOFS = new GBReg(0x014,15,0,1,0,"writeonly");
      BG1VOFS = new GBReg(0x016,15,0,1,0,"writeonly");
      BG2HOFS = new GBReg(0x018,15,0,1,0,"writeonly");
      BG2VOFS = new GBReg(0x01A,15,0,1,0,"writeonly");
      BG3HOFS = new GBReg(0x01C,15,0,1,0,"writeonly");
      BG3VOFS = new GBReg(0x01E,15,0,1,0,"writeonly");
      BG2RotScaleParDX = new GBReg(0x020,15,0,1,256,"writeonly");
      BG2RotScaleParDMX = new GBReg(0x020,31,16,1,0,"writeonly");
      BG2RotScaleParDY = new GBReg(0x024,15,0,1,0,"writeonly");
      BG2RotScaleParDMY = new GBReg(0x024,31,16,1,256,"writeonly");
      BG2RefX = new GBReg(0x028,27,0,1,0,"writeonly");
      BG2RefY = new GBReg(0x02C,27,0,1,0,"writeonly");
      BG3RotScaleParDX = new GBReg(0x030,15,0,1,256,"writeonly");
      BG3RotScaleParDMX = new GBReg(0x030,31,16,1,0,"writeonly");
      BG3RotScaleParDY = new GBReg(0x034,15,0,1,0,"writeonly");
      BG3RotScaleParDMY = new GBReg(0x034,31,16,1,256,"writeonly");
      BG3RefX = new GBReg(0x038,27,0,1,0,"writeonly");
      BG3RefY = new GBReg(0x03C,27,0,1,0,"writeonly");
      WIN0H = new GBReg(0x040,15,0,1,0,"writeonly");
      WIN0H_X2 = new GBReg(0x040,7,0,1,0,"writeonly");
      WIN0H_X1 = new GBReg(0x040,15,8,1,0,"writeonly");
      WIN1H = new GBReg(0x040,31,16,1,0,"writeonly");
      WIN1H_X2 = new GBReg(0x040,23,16,1,0,"writeonly");
      WIN1H_X1 = new GBReg(0x040,31,24,1,0,"writeonly");
      WIN0V = new GBReg(0x044,15,0,1,0,"writeonly");
      WIN0V_Y2 = new GBReg(0x044,7,0,1,0,"writeonly");
      WIN0V_Y1 = new GBReg(0x044,15,8,1,0,"writeonly");
      WIN1V = new GBReg(0x044,31,16,1,0,"writeonly");
      WIN1V_Y2 = new GBReg(0x044,23,16,1,0,"writeonly");
      WIN1V_Y1 = new GBReg(0x044,31,24,1,0,"writeonly");
      WININ = new GBReg(0x048,15,0,1,0,"writeonly");
      WININ_Window_0_BG0_Enable = new GBReg(0x048,0,0,1,0,"readwrite");
      WININ_Window_0_BG1_Enable = new GBReg(0x048,1,1,1,0,"readwrite");
      WININ_Window_0_BG2_Enable = new GBReg(0x048,2,2,1,0,"readwrite");
      WININ_Window_0_BG3_Enable = new GBReg(0x048,3,3,1,0,"readwrite");
      WININ_Window_0_OBJ_Enable = new GBReg(0x048,4,4,1,0,"readwrite");
      WININ_Window_0_Special_Effect = new GBReg(0x048,5,5,1,0,"readwrite");
      WININ_Window_1_BG0_Enable = new GBReg(0x048,8,8,1,0,"readwrite");
      WININ_Window_1_BG1_Enable = new GBReg(0x048,9,9,1,0,"readwrite");
      WININ_Window_1_BG2_Enable = new GBReg(0x048,10,10,1,0,"readwrite");
      WININ_Window_1_BG3_Enable = new GBReg(0x048,11,11,1,0,"readwrite");
      WININ_Window_1_OBJ_Enable = new GBReg(0x048,12,12,1,0,"readwrite");
      WININ_Window_1_Special_Effect = new GBReg(0x048,13,13,1,0,"readwrite");
      WINOUT = new GBReg(0x048,31,16,1,0,"writeonly");
      WINOUT_Outside_BG0_Enable = new GBReg(0x048,16,16,1,0,"readwrite");
      WINOUT_Outside_BG1_Enable = new GBReg(0x048,17,17,1,0,"readwrite");
      WINOUT_Outside_BG2_Enable = new GBReg(0x048,18,18,1,0,"readwrite");
      WINOUT_Outside_BG3_Enable = new GBReg(0x048,19,19,1,0,"readwrite");
      WINOUT_Outside_OBJ_Enable = new GBReg(0x048,20,20,1,0,"readwrite");
      WINOUT_Outside_Special_Effect = new GBReg(0x048,21,21,1,0,"readwrite");
      WINOUT_Objwnd_BG0_Enable = new GBReg(0x048,24,24,1,0,"readwrite");
      WINOUT_Objwnd_BG1_Enable = new GBReg(0x048,25,25,1,0,"readwrite");
      WINOUT_Objwnd_BG2_Enable = new GBReg(0x048,26,26,1,0,"readwrite");
      WINOUT_Objwnd_BG3_Enable = new GBReg(0x048,27,27,1,0,"readwrite");
      WINOUT_Objwnd_OBJ_Enable = new GBReg(0x048,28,28,1,0,"readwrite");
      WINOUT_Objwnd_Special_Effect = new GBReg(0x048,29,29,1,0,"readwrite");
      MOSAIC = new GBReg(0x04C,15,0,1,0,"writeonly");
      MOSAIC_BG_Mosaic_H_Size = new GBReg(0x04C,3,0,1,0,"writeonly");
      MOSAIC_BG_Mosaic_V_Size = new GBReg(0x04C,7,4,1,0,"writeonly");
      MOSAIC_OBJ_Mosaic_H_Size = new GBReg(0x04C,11,8,1,0,"writeonly");
      MOSAIC_OBJ_Mosaic_V_Size = new GBReg(0x04C,15,12,1,0,"writeonly");
      BLDCNT = new GBReg(0x050,13,0,1,0,"readwrite");
      BLDCNT_BG0_1st_Target_Pixel = new GBReg(0x050,0,0,1,0,"readwrite");
      BLDCNT_BG1_1st_Target_Pixel = new GBReg(0x050,1,1,1,0,"readwrite");
      BLDCNT_BG2_1st_Target_Pixel = new GBReg(0x050,2,2,1,0,"readwrite");
      BLDCNT_BG3_1st_Target_Pixel = new GBReg(0x050,3,3,1,0,"readwrite");
      BLDCNT_OBJ_1st_Target_Pixel = new GBReg(0x050,4,4,1,0,"readwrite");
      BLDCNT_BD_1st_Target_Pixel = new GBReg(0x050,5,5,1,0,"readwrite");
      BLDCNT_Color_Special_Effect = new GBReg(0x050,7,6,1,0,"readwrite");
      BLDCNT_BG0_2nd_Target_Pixel = new GBReg(0x050,8,8,1,0,"readwrite");
      BLDCNT_BG1_2nd_Target_Pixel = new GBReg(0x050,9,9,1,0,"readwrite");
      BLDCNT_BG2_2nd_Target_Pixel = new GBReg(0x050,10,10,1,0,"readwrite");
      BLDCNT_BG3_2nd_Target_Pixel = new GBReg(0x050,11,11,1,0,"readwrite");
      BLDCNT_OBJ_2nd_Target_Pixel = new GBReg(0x050,12,12,1,0,"readwrite");
      BLDCNT_BD_2nd_Target_Pixel = new GBReg(0x050,13,13,1,0,"readwrite");
      BLDALPHA = new GBReg(0x050,28,16,1,0,"writeonly");
      BLDALPHA_EVA_Coefficient = new GBReg(0x050,20,16,1,0,"readwrite");
      BLDALPHA_EVB_Coefficient = new GBReg(0x050,28,24,1,0,"readwrite");
      BLDY = new GBReg(0x054,4,0,1,0,"writeonly");
   }
}

public class RegSect_sound
{
   /// <summary>
   /// Channel 1 Sweep register (NR10)
   /// </summary>
   public GBReg SOUND1CNT_L;
   /// <summary>
   /// 0-2 R/W (n=0-7)
   /// </summary>
   public GBReg SOUND1CNT_L_Number_of_sweep_shift;
   /// <summary>
   /// 3 R/W (0=Increase, 1=Decrease)
   /// </summary>
   public GBReg SOUND1CNT_L_Sweep_Frequency_Direction;
   /// <summary>
   /// 4-6 R/W units of 7.8ms (0-7, min=7.8ms, max=54.7ms)
   /// </summary>
   public GBReg SOUND1CNT_L_Sweep_Time;
   /// <summary>
   /// Channel 1 Duty/Length/Envelope (NR11, NR12)
   /// </summary>
   public GBReg SOUND1CNT_H;
   /// <summary>
   /// 0-5 W units of (64-n)/256s (0-63)
   /// </summary>
   public GBReg SOUND1CNT_H_Sound_length;
   /// <summary>
   /// 6-7 R/W (0-3, see below)
   /// </summary>
   public GBReg SOUND1CNT_H_Wave_Pattern_Duty;
   /// <summary>
   /// 8-10 R/W units of n/64s (1-7, 0=No Envelope)
   /// </summary>
   public GBReg SOUND1CNT_H_Envelope_Step_Time;
   /// <summary>
   /// 11 R/W (0=Decrease, 1=Increase)
   /// </summary>
   public GBReg SOUND1CNT_H_Envelope_Direction;
   /// <summary>
   /// 12-15 R/W (1-15, 0=No Sound)
   /// </summary>
   public GBReg SOUND1CNT_H_Initial_Volume_of_envelope;
   /// <summary>
   /// Channel 1 Frequency/Control (NR13, NR14)
   /// </summary>
   public GBReg SOUND1CNT_X;
   /// <summary>
   /// 0-10 W 131072/(2048-n)Hz (0-2047)
   /// </summary>
   public GBReg SOUND1CNT_X_Frequency;
   /// <summary>
   /// 14 R/W (1=Stop output when length in NR11 expires)
   /// </summary>
   public GBReg SOUND1CNT_X_Length_Flag;
   /// <summary>
   /// 15 W (1=Restart Sound)
   /// </summary>
   public GBReg SOUND1CNT_X_Initial;
   /// <summary>
   /// must return zero
   /// </summary>
   public GBReg SOUND1CNT_XHighZero;
   /// <summary>
   /// Channel 2 Duty/Length/Envelope (NR21, NR22)
   /// </summary>
   public GBReg SOUND2CNT_L;
   /// <summary>
   /// 0-5 W units of (64-n)/256s (0-63)
   /// </summary>
   public GBReg SOUND2CNT_L_Sound_length;
   /// <summary>
   /// 6-7 R/W (0-3, see below)
   /// </summary>
   public GBReg SOUND2CNT_L_Wave_Pattern_Duty;
   /// <summary>
   /// 8-10 R/W units of n/64s (1-7, 0=No Envelope)
   /// </summary>
   public GBReg SOUND2CNT_L_Envelope_Step_Time;
   /// <summary>
   /// 11 R/W (0=Decrease, 1=Increase)
   /// </summary>
   public GBReg SOUND2CNT_L_Envelope_Direction;
   /// <summary>
   /// 12-15 R/W (1-15, 0=No Sound)
   /// </summary>
   public GBReg SOUND2CNT_L_Initial_Volume_of_envelope;
   /// <summary>
   /// Channel 2 Frequency/Control (NR23, NR24)
   /// </summary>
   public GBReg SOUND2CNT_H;
   /// <summary>
   /// 0-10 W 131072/(2048-n)Hz (0-2047)
   /// </summary>
   public GBReg SOUND2CNT_H_Frequency;
   /// <summary>
   /// 14 R/W (1=Stop output when length in NR11 expires)
   /// </summary>
   public GBReg SOUND2CNT_H_Length_Flag;
   /// <summary>
   /// 15 W (1=Restart Sound)
   /// </summary>
   public GBReg SOUND2CNT_H_Initial;
   /// <summary>
   /// must return zero
   /// </summary>
   public GBReg SOUND2CNT_HHighZero;
   /// <summary>
   /// Channel 3 Stop/Wave RAM select (NR30)
   /// </summary>
   public GBReg SOUND3CNT_L;
   /// <summary>
   /// 5 R/W (0=One bank/32 digits, 1=Two banks/64 digits)
   /// </summary>
   public GBReg SOUND3CNT_L_Wave_RAM_Dimension;
   /// <summary>
   /// 6 R/W (0-1, see below)
   /// </summary>
   public GBReg SOUND3CNT_L_Wave_RAM_Bank_Number;
   /// <summary>
   /// 7 R/W (0=Stop, 1=Playback)
   /// </summary>
   public GBReg SOUND3CNT_L_Sound_Channel_3_Off;
   /// <summary>
   /// Channel 3 Length/Volume (NR31, NR32)
   /// </summary>
   public GBReg SOUND3CNT_H;
   /// <summary>
   /// 0-7 W units of (256-n)/256s (0-255)
   /// </summary>
   public GBReg SOUND3CNT_H_Sound_length;
   /// <summary>
   /// 13-14 R/W (0=Mute/Zero, 1=100%, 2=50%, 3=25%)
   /// </summary>
   public GBReg SOUND3CNT_H_Sound_Volume;
   /// <summary>
   /// 15 R/W (0=Use above, 1=Force 75% regardless of above)
   /// </summary>
   public GBReg SOUND3CNT_H_Force_Volume;
   /// <summary>
   /// Channel 3 Frequency/Control (NR33, NR34)
   /// </summary>
   public GBReg SOUND3CNT_X;
   /// <summary>
   /// 0-10 W 2097152/(2048-n) Hz (0-2047)
   /// </summary>
   public GBReg SOUND3CNT_X_Sample_Rate;
   /// <summary>
   /// 14 R/W (1=Stop output when length in NR31 expires)
   /// </summary>
   public GBReg SOUND3CNT_X_Length_Flag;
   /// <summary>
   /// 15 W (1=Restart Sound)
   /// </summary>
   public GBReg SOUND3CNT_X_Initial;
   /// <summary>
   /// must return zero
   /// </summary>
   public GBReg SOUND3CNT_XHighZero;
   /// <summary>
   /// Channel 4 Length/Envelope (NR41, NR42)
   /// </summary>
   public GBReg SOUND4CNT_L;
   /// <summary>
   /// 0-5 W units of (64-n)/256s (0-63)
   /// </summary>
   public GBReg SOUND4CNT_L_Sound_length;
   /// <summary>
   /// 8-10 R/W units of n/64s (1-7, 0=No Envelope)
   /// </summary>
   public GBReg SOUND4CNT_L_Envelope_Step_Time;
   /// <summary>
   /// 11 R/W (0=Decrease, 1=Increase)
   /// </summary>
   public GBReg SOUND4CNT_L_Envelope_Direction;
   /// <summary>
   /// 12-15 R/W (1-15, 0=No Sound)
   /// </summary>
   public GBReg SOUND4CNT_L_Initial_Volume_of_envelope;
   /// <summary>
   /// must return zero
   /// </summary>
   public GBReg SOUND4CNT_LHighZero;
   /// <summary>
   /// Channel 4 Frequency/Control (NR43, NR44)
   /// </summary>
   public GBReg SOUND4CNT_H;
   /// <summary>
   /// 0-2 R/W (r) 524288 Hz / r / 2^(s+1) ;For r=0 assume r=0.5 instead
   /// </summary>
   public GBReg SOUND4CNT_H_Dividing_Ratio_of_Freq;
   /// <summary>
   /// 3 R/W (0=15 bits, 1=7 bits)
   /// </summary>
   public GBReg SOUND4CNT_H_Counter_Step_Width;
   /// <summary>
   /// 4-7 R/W (s) 524288 Hz / r / 2^(s+1) ;For r=0 assume r=0.5 instead
   /// </summary>
   public GBReg SOUND4CNT_H_Shift_Clock_Frequency;
   /// <summary>
   /// 14 R/W (1=Stop output when length in NR41 expires)
   /// </summary>
   public GBReg SOUND4CNT_H_Length_Flag;
   /// <summary>
   /// 15 W (1=Restart Sound)
   /// </summary>
   public GBReg SOUND4CNT_H_Initial;
   /// <summary>
   /// must return zero
   /// </summary>
   public GBReg SOUND4CNT_HHighZero;
   /// <summary>
   /// Control Stereo/Volume/Enable (NR50, NR51)
   /// </summary>
   public GBReg SOUNDCNT_L;
   /// <summary>
   /// 0-2 (0-7)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_1_4_Master_Volume_RIGHT;
   /// <summary>
   /// 4-6 (0-7)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_1_4_Master_Volume_LEFT;
   /// <summary>
   /// 8-11 (each Bit 8-11, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_1_Enable_Flags_RIGHT;
   /// <summary>
   /// 8-11 (each Bit 8-11, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_2_Enable_Flags_RIGHT;
   /// <summary>
   /// 8-11 (each Bit 8-11, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_3_Enable_Flags_RIGHT;
   /// <summary>
   /// 8-11 (each Bit 8-11, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_4_Enable_Flags_RIGHT;
   /// <summary>
   /// 12-15 (each Bit 12-15, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_1_Enable_Flags_LEFT;
   /// <summary>
   /// 12-15 (each Bit 12-15, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_2_Enable_Flags_LEFT;
   /// <summary>
   /// 12-15 (each Bit 12-15, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_3_Enable_Flags_LEFT;
   /// <summary>
   /// 12-15 (each Bit 12-15, 0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_L_Sound_4_Enable_Flags_LEFT;
   /// <summary>
   /// Control Mixing/DMA Control
   /// </summary>
   public GBReg SOUNDCNT_H;
   /// <summary>
   /// 0-1 Sound # 1-4 Volume (0=25%, 1=50%, 2=100%, 3=Prohibited)
   /// </summary>
   public GBReg SOUNDCNT_H_Sound_1_4_Volume;
   /// <summary>
   /// 2 DMA Sound A Volume (0=50%, 1=100%)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_A_Volume;
   /// <summary>
   /// 3 DMA Sound B Volume (0=50%, 1=100%)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_B_Volume;
   /// <summary>
   /// 8 DMA Sound A Enable RIGHT (0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_A_Enable_RIGHT;
   /// <summary>
   /// 9 DMA Sound A Enable LEFT (0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_A_Enable_LEFT;
   /// <summary>
   /// 10 DMA Sound A Timer Select (0=Timer 0, 1=Timer 1)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_A_Timer_Select;
   /// <summary>
   /// 11 DMA Sound A Reset FIFO (1=Reset)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_A_Reset_FIFO;
   /// <summary>
   /// 12 DMA Sound B Enable RIGHT (0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_B_Enable_RIGHT;
   /// <summary>
   /// 13 DMA Sound B Enable LEFT (0=Disable, 1=Enable)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_B_Enable_LEFT;
   /// <summary>
   /// 14 DMA Sound B Timer Select (0=Timer 0, 1=Timer 1)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_B_Timer_Select;
   /// <summary>
   /// 15 DMA Sound B Reset FIFO (1=Reset)
   /// </summary>
   public GBReg SOUNDCNT_H_DMA_Sound_B_Reset_FIFO;
   /// <summary>
   /// Control Sound on/off (NR52)
   /// </summary>
   public GBReg SOUNDCNT_X;
   /// <summary>
   /// 0 (Read Only)
   /// </summary>
   public GBReg SOUNDCNT_X_Sound_1_ON_flag;
   /// <summary>
   /// 1 (Read Only)
   /// </summary>
   public GBReg SOUNDCNT_X_Sound_2_ON_flag;
   /// <summary>
   /// 2 (Read Only)
   /// </summary>
   public GBReg SOUNDCNT_X_Sound_3_ON_flag;
   /// <summary>
   /// 3 (Read Only)
   /// </summary>
   public GBReg SOUNDCNT_X_Sound_4_ON_flag;
   /// <summary>
   /// 7 (0=Disable, 1=Enable) (Read/Write)
   /// </summary>
   public GBReg SOUNDCNT_X_PSG_FIFO_Master_Enable;
   /// <summary>
   /// must return zero
   /// </summary>
   public GBReg SOUNDCNT_XHighZero;
   /// <summary>
   /// Sound PWM Control (R/W)
   /// </summary>
   public GBReg SOUNDBIAS;
   /// <summary>
   /// 0-9 (Default=200h, converting signed samples into unsigned)
   /// </summary>
   public GBReg SOUNDBIAS_Bias_Level;
   /// <summary>
   /// 14-15 (Default=0, see below)
   /// </summary>
   public GBReg SOUNDBIAS_Amp_Res_Sampling_Cycle;
   /// <summary>
   /// must return zero
   /// </summary>
   public GBReg SOUNDBIAS_HighZero;
   /// <summary>
   /// Channel 3 Wave Pattern RAM (2 banks!!)
   /// </summary>
   public GBReg WAVE_RAM;
   /// <summary>
   /// Channel 3 Wave Pattern RAM (2 banks!!)
   /// </summary>
   public GBReg WAVE_RAM2;
   /// <summary>
   /// Channel 3 Wave Pattern RAM (2 banks!!)
   /// </summary>
   public GBReg WAVE_RAM3;
   /// <summary>
   /// Channel 3 Wave Pattern RAM (2 banks!!)
   /// </summary>
   public GBReg WAVE_RAM4;
   /// <summary>
   /// Channel A FIFO, Data 0-3
   /// </summary>
   public GBReg FIFO_A;
   /// <summary>
   /// Channel B FIFO, Data 0-3
   /// </summary>
   public GBReg FIFO_B;

   public RegSect_sound() 
   {
      SOUND1CNT_L = new GBReg(0x060,6,0,1,0,"readwrite");
      SOUND1CNT_L_Number_of_sweep_shift = new GBReg(0x060,2,0,1,0,"readwrite");
      SOUND1CNT_L_Sweep_Frequency_Direction = new GBReg(0x060,3,3,1,0,"readwrite");
      SOUND1CNT_L_Sweep_Time = new GBReg(0x060,6,4,1,0,"readwrite");
      SOUND1CNT_H = new GBReg(0x060,31,16,1,0,"writeonly");
      SOUND1CNT_H_Sound_length = new GBReg(0x060,21,16,1,0,"writeonly");
      SOUND1CNT_H_Wave_Pattern_Duty = new GBReg(0x060,23,22,1,0,"readwrite");
      SOUND1CNT_H_Envelope_Step_Time = new GBReg(0x060,26,24,1,0,"readwrite");
      SOUND1CNT_H_Envelope_Direction = new GBReg(0x060,27,27,1,0,"readwrite");
      SOUND1CNT_H_Initial_Volume_of_envelope = new GBReg(0x060,31,28,1,0,"readwrite");
      SOUND1CNT_X = new GBReg(0x064,15,0,1,0,"writeonly");
      SOUND1CNT_X_Frequency = new GBReg(0x064,10,0,1,0,"writeonly");
      SOUND1CNT_X_Length_Flag = new GBReg(0x064,14,14,1,0,"readwrite");
      SOUND1CNT_X_Initial = new GBReg(0x064,15,15,1,0,"writeonly");
      SOUND1CNT_XHighZero = new GBReg(0x064,31,16,1,0,"readonly");
      SOUND2CNT_L = new GBReg(0x068,15,0,1,0,"writeonly");
      SOUND2CNT_L_Sound_length = new GBReg(0x068,5,0,1,0,"writeonly");
      SOUND2CNT_L_Wave_Pattern_Duty = new GBReg(0x068,7,6,1,0,"readwrite");
      SOUND2CNT_L_Envelope_Step_Time = new GBReg(0x068,10,8,1,0,"readwrite");
      SOUND2CNT_L_Envelope_Direction = new GBReg(0x068,11,11,1,0,"readwrite");
      SOUND2CNT_L_Initial_Volume_of_envelope = new GBReg(0x068,15,12,1,0,"readwrite");
      SOUND2CNT_H = new GBReg(0x06C,15,0,1,0,"writeonly");
      SOUND2CNT_H_Frequency = new GBReg(0x06C,10,0,1,0,"writeonly");
      SOUND2CNT_H_Length_Flag = new GBReg(0x06C,14,14,1,0,"readwrite");
      SOUND2CNT_H_Initial = new GBReg(0x06C,15,15,1,0,"writeonly");
      SOUND2CNT_HHighZero = new GBReg(0x06C,31,16,1,0,"readonly");
      SOUND3CNT_L = new GBReg(0x070,15,0,1,0,"writeonly");
      SOUND3CNT_L_Wave_RAM_Dimension = new GBReg(0x070,5,5,1,0,"readwrite");
      SOUND3CNT_L_Wave_RAM_Bank_Number = new GBReg(0x070,6,6,1,0,"readwrite");
      SOUND3CNT_L_Sound_Channel_3_Off = new GBReg(0x070,7,7,1,0,"readwrite");
      SOUND3CNT_H = new GBReg(0x070,31,16,1,0,"writeonly");
      SOUND3CNT_H_Sound_length = new GBReg(0x070,23,16,1,0,"writeonly");
      SOUND3CNT_H_Sound_Volume = new GBReg(0x070,30,29,1,0,"readwrite");
      SOUND3CNT_H_Force_Volume = new GBReg(0x070,31,31,1,0,"readwrite");
      SOUND3CNT_X = new GBReg(0x074,15,0,1,0,"writeonly");
      SOUND3CNT_X_Sample_Rate = new GBReg(0x074,10,0,1,0,"writeonly");
      SOUND3CNT_X_Length_Flag = new GBReg(0x074,14,14,1,0,"readwrite");
      SOUND3CNT_X_Initial = new GBReg(0x074,15,15,1,0,"writeonly");
      SOUND3CNT_XHighZero = new GBReg(0x074,31,16,1,0,"readonly");
      SOUND4CNT_L = new GBReg(0x078,15,0,1,0,"writeonly");
      SOUND4CNT_L_Sound_length = new GBReg(0x078,5,0,1,0,"writeonly");
      SOUND4CNT_L_Envelope_Step_Time = new GBReg(0x078,10,8,1,0,"readwrite");
      SOUND4CNT_L_Envelope_Direction = new GBReg(0x078,11,11,1,0,"readwrite");
      SOUND4CNT_L_Initial_Volume_of_envelope = new GBReg(0x078,15,12,1,0,"readwrite");
      SOUND4CNT_LHighZero = new GBReg(0x078,31,16,1,0,"readonly");
      SOUND4CNT_H = new GBReg(0x07C,15,0,1,0,"writeonly");
      SOUND4CNT_H_Dividing_Ratio_of_Freq = new GBReg(0x07C,2,0,1,0,"readwrite");
      SOUND4CNT_H_Counter_Step_Width = new GBReg(0x07C,3,3,1,0,"readwrite");
      SOUND4CNT_H_Shift_Clock_Frequency = new GBReg(0x07C,7,4,1,0,"readwrite");
      SOUND4CNT_H_Length_Flag = new GBReg(0x07C,14,14,1,0,"readwrite");
      SOUND4CNT_H_Initial = new GBReg(0x07C,15,15,1,0,"writeonly");
      SOUND4CNT_HHighZero = new GBReg(0x07C,31,16,1,0,"readonly");
      SOUNDCNT_L = new GBReg(0x080,15,0,1,0,"writeonly");
      SOUNDCNT_L_Sound_1_4_Master_Volume_RIGHT = new GBReg(0x080,2,0,1,0,"readwrite");
      SOUNDCNT_L_Sound_1_4_Master_Volume_LEFT = new GBReg(0x080,6,4,1,0,"readwrite");
      SOUNDCNT_L_Sound_1_Enable_Flags_RIGHT = new GBReg(0x080,8,8,1,0,"readwrite");
      SOUNDCNT_L_Sound_2_Enable_Flags_RIGHT = new GBReg(0x080,9,9,1,0,"readwrite");
      SOUNDCNT_L_Sound_3_Enable_Flags_RIGHT = new GBReg(0x080,10,10,1,0,"readwrite");
      SOUNDCNT_L_Sound_4_Enable_Flags_RIGHT = new GBReg(0x080,11,11,1,0,"readwrite");
      SOUNDCNT_L_Sound_1_Enable_Flags_LEFT = new GBReg(0x080,12,12,1,0,"readwrite");
      SOUNDCNT_L_Sound_2_Enable_Flags_LEFT = new GBReg(0x080,13,13,1,0,"readwrite");
      SOUNDCNT_L_Sound_3_Enable_Flags_LEFT = new GBReg(0x080,14,14,1,0,"readwrite");
      SOUNDCNT_L_Sound_4_Enable_Flags_LEFT = new GBReg(0x080,15,15,1,0,"readwrite");
      SOUNDCNT_H = new GBReg(0x080,31,16,1,0,"readwrite");
      SOUNDCNT_H_Sound_1_4_Volume = new GBReg(0x080,17,16,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_A_Volume = new GBReg(0x080,18,18,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_B_Volume = new GBReg(0x080,19,19,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_A_Enable_RIGHT = new GBReg(0x080,24,24,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_A_Enable_LEFT = new GBReg(0x080,25,25,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_A_Timer_Select = new GBReg(0x080,26,26,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_A_Reset_FIFO = new GBReg(0x080,27,27,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_B_Enable_RIGHT = new GBReg(0x080,28,28,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_B_Enable_LEFT = new GBReg(0x080,29,29,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_B_Timer_Select = new GBReg(0x080,30,30,1,0,"readwrite");
      SOUNDCNT_H_DMA_Sound_B_Reset_FIFO = new GBReg(0x080,31,31,1,0,"readwrite");
      SOUNDCNT_X = new GBReg(0x084,7,0,1,0,"readwrite");
      SOUNDCNT_X_Sound_1_ON_flag = new GBReg(0x084,0,0,1,0,"readwrite");
      SOUNDCNT_X_Sound_2_ON_flag = new GBReg(0x084,1,1,1,0,"readwrite");
      SOUNDCNT_X_Sound_3_ON_flag = new GBReg(0x084,2,2,1,0,"readwrite");
      SOUNDCNT_X_Sound_4_ON_flag = new GBReg(0x084,3,3,1,0,"readwrite");
      SOUNDCNT_X_PSG_FIFO_Master_Enable = new GBReg(0x084,7,7,1,0,"readwrite");
      SOUNDCNT_XHighZero = new GBReg(0x084,31,16,1,0,"readonly");
      SOUNDBIAS = new GBReg(0x088,15,0,1,0x0200,"readwrite");
      SOUNDBIAS_Bias_Level = new GBReg(0x088,9,0,1,0,"readwrite");
      SOUNDBIAS_Amp_Res_Sampling_Cycle = new GBReg(0x088,15,14,1,0,"readwrite");
      SOUNDBIAS_HighZero = new GBReg(0x088,31,16,1,0,"readonly");
      WAVE_RAM = new GBReg(0x090,31,0,4,0,"readwrite");
      WAVE_RAM2 = new GBReg(0x094,31,0,1,0,"readwrite");
      WAVE_RAM3 = new GBReg(0x098,31,0,1,0,"readwrite");
      WAVE_RAM4 = new GBReg(0x09C,31,0,1,0,"readwrite");
      FIFO_A = new GBReg(0x0A0,31,0,1,0,"writeonly");
      FIFO_B = new GBReg(0x0A4,31,0,1,0,"writeonly");
   }
}

public class RegSect_dma
{
   /// <summary>
   /// Source Address 4 W
   /// </summary>
   public GBReg DMA0SAD;
   /// <summary>
   /// Destination Address 4 W
   /// </summary>
   public GBReg DMA0DAD;
   /// <summary>
   /// Word Count 2 W
   /// </summary>
   public GBReg DMA0CNT_L;
   /// <summary>
   /// Control 2 R/W
   /// </summary>
   public GBReg DMA0CNT_H;
   /// <summary>
   /// 5-6 Dest Addr Control (0=Increment,1=Decrement,2=Fixed,3=Increment/Reload)
   /// </summary>
   public GBReg DMA0CNT_H_Dest_Addr_Control;
   /// <summary>
   /// 7-8 Source Adr Control (0=Increment,1=Decrement,2=Fixed,3=Prohibited)
   /// </summary>
   public GBReg DMA0CNT_H_Source_Adr_Control;
   /// <summary>
   /// 9 DMA Repeat (0=Off, 1=On) (Must be zero if Bit 11 set)
   /// </summary>
   public GBReg DMA0CNT_H_DMA_Repeat;
   /// <summary>
   /// 10 DMA Transfer Type (0=16bit, 1=32bit)
   /// </summary>
   public GBReg DMA0CNT_H_DMA_Transfer_Type;
   /// <summary>
   /// 12-13 DMA Start Timing (0=Immediately, 1=VBlank, 2=HBlank, 3=Special) The 'Special' setting (Start Timing=3) depends on the DMA channel: DMA0=Prohibited, DMA1/DMA2=Sound FIFO, DMA3=Video Capture
   /// </summary>
   public GBReg DMA0CNT_H_DMA_Start_Timing;
   /// <summary>
   /// 14 IRQ upon end of Word Count (0=Disable, 1=Enable)
   /// </summary>
   public GBReg DMA0CNT_H_IRQ_on;
   /// <summary>
   /// 15 DMA Enable (0=Off, 1=On)
   /// </summary>
   public GBReg DMA0CNT_H_DMA_Enable;
   /// <summary>
   /// Source Address 4 W
   /// </summary>
   public GBReg DMA1SAD;
   /// <summary>
   /// Destination Address 4 W
   /// </summary>
   public GBReg DMA1DAD;
   /// <summary>
   /// Word Count 2 W
   /// </summary>
   public GBReg DMA1CNT_L;
   /// <summary>
   /// Control 2 R/W
   /// </summary>
   public GBReg DMA1CNT_H;
   /// <summary>
   /// 5-6 Dest Addr Control (0=Increment,1=Decrement,2=Fixed,3=Increment/Reload)
   /// </summary>
   public GBReg DMA1CNT_H_Dest_Addr_Control;
   /// <summary>
   /// 7-8 Source Adr Control (0=Increment,1=Decrement,2=Fixed,3=Prohibited)
   /// </summary>
   public GBReg DMA1CNT_H_Source_Adr_Control;
   /// <summary>
   /// 9 DMA Repeat (0=Off, 1=On) (Must be zero if Bit 11 set)
   /// </summary>
   public GBReg DMA1CNT_H_DMA_Repeat;
   /// <summary>
   /// 10 DMA Transfer Type (0=16bit, 1=32bit)
   /// </summary>
   public GBReg DMA1CNT_H_DMA_Transfer_Type;
   /// <summary>
   /// 12-13 DMA Start Timing (0=Immediately, 1=VBlank, 2=HBlank, 3=Special) The 'Special' setting (Start Timing=3) depends on the DMA channel: DMA0=Prohibited, DMA1/DMA2=Sound FIFO, DMA3=Video Capture
   /// </summary>
   public GBReg DMA1CNT_H_DMA_Start_Timing;
   /// <summary>
   /// 14 IRQ upon end of Word Count (0=Disable, 1=Enable)
   /// </summary>
   public GBReg DMA1CNT_H_IRQ_on;
   /// <summary>
   /// 15 DMA Enable (0=Off, 1=On)
   /// </summary>
   public GBReg DMA1CNT_H_DMA_Enable;
   /// <summary>
   /// Source Address 4 W
   /// </summary>
   public GBReg DMA2SAD;
   /// <summary>
   /// Destination Address 4 W
   /// </summary>
   public GBReg DMA2DAD;
   /// <summary>
   /// Word Count 2 W
   /// </summary>
   public GBReg DMA2CNT_L;
   /// <summary>
   /// Control 2 R/W
   /// </summary>
   public GBReg DMA2CNT_H;
   /// <summary>
   /// 5-6 Dest Addr Control (0=Increment,1=Decrement,2=Fixed,3=Increment/Reload)
   /// </summary>
   public GBReg DMA2CNT_H_Dest_Addr_Control;
   /// <summary>
   /// 7-8 Source Adr Control (0=Increment,1=Decrement,2=Fixed,3=Prohibited)
   /// </summary>
   public GBReg DMA2CNT_H_Source_Adr_Control;
   /// <summary>
   /// 9 DMA Repeat (0=Off, 1=On) (Must be zero if Bit 11 set)
   /// </summary>
   public GBReg DMA2CNT_H_DMA_Repeat;
   /// <summary>
   /// 10 DMA Transfer Type (0=16bit, 1=32bit)
   /// </summary>
   public GBReg DMA2CNT_H_DMA_Transfer_Type;
   /// <summary>
   /// 12-13 DMA Start Timing (0=Immediately, 1=VBlank, 2=HBlank, 3=Special) The 'Special' setting (Start Timing=3) depends on the DMA channel: DMA0=Prohibited, DMA1/DMA2=Sound FIFO, DMA3=Video Capture
   /// </summary>
   public GBReg DMA2CNT_H_DMA_Start_Timing;
   /// <summary>
   /// 14 IRQ upon end of Word Count (0=Disable, 1=Enable)
   /// </summary>
   public GBReg DMA2CNT_H_IRQ_on;
   /// <summary>
   /// 15 DMA Enable (0=Off, 1=On)
   /// </summary>
   public GBReg DMA2CNT_H_DMA_Enable;
   /// <summary>
   /// Source Address 4 W
   /// </summary>
   public GBReg DMA3SAD;
   /// <summary>
   /// Destination Address 4 W
   /// </summary>
   public GBReg DMA3DAD;
   /// <summary>
   /// Word Count 2 W
   /// </summary>
   public GBReg DMA3CNT_L;
   /// <summary>
   /// Control 2 R/W
   /// </summary>
   public GBReg DMA3CNT_H;
   /// <summary>
   /// 5-6 Dest Addr Control (0=Increment,1=Decrement,2=Fixed,3=Increment/Reload)
   /// </summary>
   public GBReg DMA3CNT_H_Dest_Addr_Control;
   /// <summary>
   /// 7-8 Source Adr Control (0=Increment,1=Decrement,2=Fixed,3=Prohibited)
   /// </summary>
   public GBReg DMA3CNT_H_Source_Adr_Control;
   /// <summary>
   /// 9 DMA Repeat (0=Off, 1=On) (Must be zero if Bit 11 set)
   /// </summary>
   public GBReg DMA3CNT_H_DMA_Repeat;
   /// <summary>
   /// 10 DMA Transfer Type (0=16bit, 1=32bit)
   /// </summary>
   public GBReg DMA3CNT_H_DMA_Transfer_Type;
   /// <summary>
   /// 11 Game Pak DRQ - DMA3 only - (0=Normal, 1=DRQ <from> Game Pak, DMA3)
   /// </summary>
   public GBReg DMA3CNT_H_Game_Pak_DRQ;
   /// <summary>
   /// 12-13 DMA Start Timing (0=Immediately, 1=VBlank, 2=HBlank, 3=Special) The 'Special' setting (Start Timing=3) depends on the DMA channel: DMA0=Prohibited, DMA1/DMA2=Sound FIFO, DMA3=Video Capture
   /// </summary>
   public GBReg DMA3CNT_H_DMA_Start_Timing;
   /// <summary>
   /// 14 IRQ upon end of Word Count (0=Disable, 1=Enable)
   /// </summary>
   public GBReg DMA3CNT_H_IRQ_on;
   /// <summary>
   /// 15 DMA Enable (0=Off, 1=On)
   /// </summary>
   public GBReg DMA3CNT_H_DMA_Enable;

   public RegSect_dma() 
   {
      DMA0SAD = new GBReg(0xB0,31,0,1,0,"writeonly");
      DMA0DAD = new GBReg(0xB4,31,0,1,0,"writeonly");
      DMA0CNT_L = new GBReg(0xB8,15,0,1,0,"writeonly");
      DMA0CNT_H = new GBReg(0xB8,31,16,1,0,"writeonly");
      DMA0CNT_H_Dest_Addr_Control = new GBReg(0xB8,22,21,1,0,"readwrite");
      DMA0CNT_H_Source_Adr_Control = new GBReg(0xB8,24,23,1,0,"readwrite");
      DMA0CNT_H_DMA_Repeat = new GBReg(0xB8,25,25,1,0,"readwrite");
      DMA0CNT_H_DMA_Transfer_Type = new GBReg(0xB8,26,26,1,0,"readwrite");
      DMA0CNT_H_DMA_Start_Timing = new GBReg(0xB8,29,28,1,0,"readwrite");
      DMA0CNT_H_IRQ_on = new GBReg(0xB8,30,30,1,0,"readwrite");
      DMA0CNT_H_DMA_Enable = new GBReg(0xB8,31,31,1,0,"readwrite");
      DMA1SAD = new GBReg(0xBC,31,0,1,0,"writeonly");
      DMA1DAD = new GBReg(0xC0,31,0,1,0,"writeonly");
      DMA1CNT_L = new GBReg(0xC4,15,0,1,0,"writeonly");
      DMA1CNT_H = new GBReg(0xC4,31,16,1,0,"writeonly");
      DMA1CNT_H_Dest_Addr_Control = new GBReg(0xC4,22,21,1,0,"readwrite");
      DMA1CNT_H_Source_Adr_Control = new GBReg(0xC4,24,23,1,0,"readwrite");
      DMA1CNT_H_DMA_Repeat = new GBReg(0xC4,25,25,1,0,"readwrite");
      DMA1CNT_H_DMA_Transfer_Type = new GBReg(0xC4,26,26,1,0,"readwrite");
      DMA1CNT_H_DMA_Start_Timing = new GBReg(0xC4,29,28,1,0,"readwrite");
      DMA1CNT_H_IRQ_on = new GBReg(0xC4,30,30,1,0,"readwrite");
      DMA1CNT_H_DMA_Enable = new GBReg(0xC4,31,31,1,0,"readwrite");
      DMA2SAD = new GBReg(0xC8,31,0,1,0,"writeonly");
      DMA2DAD = new GBReg(0xCC,31,0,1,0,"writeonly");
      DMA2CNT_L = new GBReg(0xD0,15,0,1,0,"writeonly");
      DMA2CNT_H = new GBReg(0xD0,31,16,1,0,"writeonly");
      DMA2CNT_H_Dest_Addr_Control = new GBReg(0xD0,22,21,1,0,"readwrite");
      DMA2CNT_H_Source_Adr_Control = new GBReg(0xD0,24,23,1,0,"readwrite");
      DMA2CNT_H_DMA_Repeat = new GBReg(0xD0,25,25,1,0,"readwrite");
      DMA2CNT_H_DMA_Transfer_Type = new GBReg(0xD0,26,26,1,0,"readwrite");
      DMA2CNT_H_DMA_Start_Timing = new GBReg(0xD0,29,28,1,0,"readwrite");
      DMA2CNT_H_IRQ_on = new GBReg(0xD0,30,30,1,0,"readwrite");
      DMA2CNT_H_DMA_Enable = new GBReg(0xD0,31,31,1,0,"readwrite");
      DMA3SAD = new GBReg(0xD4,31,0,1,0,"writeonly");
      DMA3DAD = new GBReg(0xD8,31,0,1,0,"writeonly");
      DMA3CNT_L = new GBReg(0xDC,15,0,1,0,"writeonly");
      DMA3CNT_H = new GBReg(0xDC,31,16,1,0,"writeonly");
      DMA3CNT_H_Dest_Addr_Control = new GBReg(0xDC,22,21,1,0,"readwrite");
      DMA3CNT_H_Source_Adr_Control = new GBReg(0xDC,24,23,1,0,"readwrite");
      DMA3CNT_H_DMA_Repeat = new GBReg(0xDC,25,25,1,0,"readwrite");
      DMA3CNT_H_DMA_Transfer_Type = new GBReg(0xDC,26,26,1,0,"readwrite");
      DMA3CNT_H_Game_Pak_DRQ = new GBReg(0xDC,27,27,1,0,"readwrite");
      DMA3CNT_H_DMA_Start_Timing = new GBReg(0xDC,29,28,1,0,"readwrite");
      DMA3CNT_H_IRQ_on = new GBReg(0xDC,30,30,1,0,"readwrite");
      DMA3CNT_H_DMA_Enable = new GBReg(0xDC,31,31,1,0,"readwrite");
   }
}

public class RegSect_timer
{
   /// <summary>
   /// Timer 0 Counter/Reload 2 R/W
   /// </summary>
   public GBReg TM0CNT_L;
   /// <summary>
   /// Timer 0 Control 2 R/W
   /// </summary>
   public GBReg TM0CNT_H;
   /// <summary>
   /// Prescaler Selection (0=F/1, 1=F/64, 2=F/256, 3=F/1024)
   /// </summary>
   public GBReg TM0CNT_H_Prescaler;
   /// <summary>
   /// Count-up Timing (0=Normal, 1=See below)
   /// </summary>
   public GBReg TM0CNT_H_Count_up;
   /// <summary>
   /// Timer IRQ Enable (0=Disable, 1=IRQ on Timer overflow)
   /// </summary>
   public GBReg TM0CNT_H_Timer_IRQ_Enable;
   /// <summary>
   /// Timer Start/Stop (0=Stop, 1=Operate)
   /// </summary>
   public GBReg TM0CNT_H_Timer_Start_Stop;
   /// <summary>
   /// Timer 1 Counter/Reload 2 R/W
   /// </summary>
   public GBReg TM1CNT_L;
   /// <summary>
   /// Timer 1 Control 2 R/W
   /// </summary>
   public GBReg TM1CNT_H;
   /// <summary>
   /// Prescaler Selection (0=F/1, 1=F/64, 2=F/256, 3=F/1024)
   /// </summary>
   public GBReg TM1CNT_H_Prescaler;
   /// <summary>
   /// Count-up Timing (0=Normal, 1=See below)
   /// </summary>
   public GBReg TM1CNT_H_Count_up;
   /// <summary>
   /// Timer IRQ Enable (0=Disable, 1=IRQ on Timer overflow)
   /// </summary>
   public GBReg TM1CNT_H_Timer_IRQ_Enable;
   /// <summary>
   /// Timer Start/Stop (0=Stop, 1=Operate)
   /// </summary>
   public GBReg TM1CNT_H_Timer_Start_Stop;
   /// <summary>
   /// Timer 2 Counter/Reload 2 R/W
   /// </summary>
   public GBReg TM2CNT_L;
   /// <summary>
   /// Timer 2 Control 2 R/W
   /// </summary>
   public GBReg TM2CNT_H;
   /// <summary>
   /// Prescaler Selection (0=F/1, 1=F/64, 2=F/256, 3=F/1024)
   /// </summary>
   public GBReg TM2CNT_H_Prescaler;
   /// <summary>
   /// Count-up Timing (0=Normal, 1=See below)
   /// </summary>
   public GBReg TM2CNT_H_Count_up;
   /// <summary>
   /// Timer IRQ Enable (0=Disable, 1=IRQ on Timer overflow)
   /// </summary>
   public GBReg TM2CNT_H_Timer_IRQ_Enable;
   /// <summary>
   /// Timer Start/Stop (0=Stop, 1=Operate)
   /// </summary>
   public GBReg TM2CNT_H_Timer_Start_Stop;
   /// <summary>
   /// Timer 3 Counter/Reload 2 R/W
   /// </summary>
   public GBReg TM3CNT_L;
   /// <summary>
   /// Timer 3 Control 2 R/W
   /// </summary>
   public GBReg TM3CNT_H;
   /// <summary>
   /// Prescaler Selection (0=F/1, 1=F/64, 2=F/256, 3=F/1024)
   /// </summary>
   public GBReg TM3CNT_H_Prescaler;
   /// <summary>
   /// Count-up Timing (0=Normal, 1=See below)
   /// </summary>
   public GBReg TM3CNT_H_Count_up;
   /// <summary>
   /// Timer IRQ Enable (0=Disable, 1=IRQ on Timer overflow)
   /// </summary>
   public GBReg TM3CNT_H_Timer_IRQ_Enable;
   /// <summary>
   /// Timer Start/Stop (0=Stop, 1=Operate)
   /// </summary>
   public GBReg TM3CNT_H_Timer_Start_Stop;

   public RegSect_timer() 
   {
      TM0CNT_L = new GBReg(0x100,15,0,1,0,"readwrite");
      TM0CNT_H = new GBReg(0x100,31,16,1,0,"readwrite");
      TM0CNT_H_Prescaler = new GBReg(0x100,17,16,1,0,"readwrite");
      TM0CNT_H_Count_up = new GBReg(0x100,18,18,1,0,"readwrite");
      TM0CNT_H_Timer_IRQ_Enable = new GBReg(0x100,22,22,1,0,"readwrite");
      TM0CNT_H_Timer_Start_Stop = new GBReg(0x100,23,23,1,0,"readwrite");
      TM1CNT_L = new GBReg(0x104,15,0,1,0,"readwrite");
      TM1CNT_H = new GBReg(0x104,31,16,1,0,"readwrite");
      TM1CNT_H_Prescaler = new GBReg(0x104,17,16,1,0,"readwrite");
      TM1CNT_H_Count_up = new GBReg(0x104,18,18,1,0,"readwrite");
      TM1CNT_H_Timer_IRQ_Enable = new GBReg(0x104,22,22,1,0,"readwrite");
      TM1CNT_H_Timer_Start_Stop = new GBReg(0x104,23,23,1,0,"readwrite");
      TM2CNT_L = new GBReg(0x108,15,0,1,0,"readwrite");
      TM2CNT_H = new GBReg(0x108,31,16,1,0,"readwrite");
      TM2CNT_H_Prescaler = new GBReg(0x108,17,16,1,0,"readwrite");
      TM2CNT_H_Count_up = new GBReg(0x108,18,18,1,0,"readwrite");
      TM2CNT_H_Timer_IRQ_Enable = new GBReg(0x108,22,22,1,0,"readwrite");
      TM2CNT_H_Timer_Start_Stop = new GBReg(0x108,23,23,1,0,"readwrite");
      TM3CNT_L = new GBReg(0x10C,15,0,1,0,"readwrite");
      TM3CNT_H = new GBReg(0x10C,31,16,1,0,"readwrite");
      TM3CNT_H_Prescaler = new GBReg(0x10C,17,16,1,0,"readwrite");
      TM3CNT_H_Count_up = new GBReg(0x10C,18,18,1,0,"readwrite");
      TM3CNT_H_Timer_IRQ_Enable = new GBReg(0x10C,22,22,1,0,"readwrite");
      TM3CNT_H_Timer_Start_Stop = new GBReg(0x10C,23,23,1,0,"readwrite");
   }
}

public class RegSect_serial
{
   /// <summary>
   /// SIO Data (Normal-32bit Mode; shared with below) 4 R/W
   /// </summary>
   public GBReg SIODATA32;
   /// <summary>
   /// SIO Data 0 (Parent) (Multi-Player Mode) 2 R/W
   /// </summary>
   public GBReg SIOMULTI0;
   /// <summary>
   /// SIO Data 1 (1st Child) (Multi-Player Mode) 2 R/W
   /// </summary>
   public GBReg SIOMULTI1;
   /// <summary>
   /// SIO Data 2 (2nd Child) (Multi-Player Mode) 2 R/W
   /// </summary>
   public GBReg SIOMULTI2;
   /// <summary>
   /// SIO Data 3 (3rd Child) (Multi-Player Mode) 2 R/W
   /// </summary>
   public GBReg SIOMULTI3;
   /// <summary>
   /// SIO Control Register 2 R/W
   /// </summary>
   public GBReg SIOCNT;
   /// <summary>
   /// SIO Data (Local of MultiPlayer; shared below) 2 R/W
   /// </summary>
   public GBReg SIOMLT_SEND;
   /// <summary>
   /// SIO Data (Normal-8bit and UART Mode) 2 R/W
   /// </summary>
   public GBReg SIODATA8;
   /// <summary>
   /// SIO Mode Select/General Purpose Data 2 R/W
   /// </summary>
   public GBReg RCNT;
   /// <summary>
   /// Ancient - Infrared Register (Prototypes only) - -
   /// </summary>
   public GBReg IR;
   /// <summary>
   /// SIO JOY Bus Control 2 R/W
   /// </summary>
   public GBReg JOYCNT;
   /// <summary>
   /// SIO JOY Bus Receive Data 4 R/W
   /// </summary>
   public GBReg JOY_RECV;
   /// <summary>
   /// SIO JOY Bus Transmit Data 4 R/W
   /// </summary>
   public GBReg JOY_TRANS;
   /// <summary>
   /// SIO JOY Bus Receive Status 2 R/?
   /// </summary>
   public GBReg JOYSTAT;

   public RegSect_serial() 
   {
      SIODATA32 = new GBReg(0x120,31,0,1,0,"readwrite");
      SIOMULTI0 = new GBReg(0x120,15,0,1,0,"readwrite");
      SIOMULTI1 = new GBReg(0x122,15,0,1,0,"readwrite");
      SIOMULTI2 = new GBReg(0x124,15,0,1,0,"readwrite");
      SIOMULTI3 = new GBReg(0x126,15,0,1,0,"readwrite");
      SIOCNT = new GBReg(0x128,15,0,1,0,"readwrite");
      SIOMLT_SEND = new GBReg(0x12A,15,0,1,0,"readwrite");
      SIODATA8 = new GBReg(0x12A,15,0,1,0,"readwrite");
      RCNT = new GBReg(0x134,15,0,1,0,"readwrite");
      IR = new GBReg(0x136,15,0,1,0,"readwrite");
      JOYCNT = new GBReg(0x140,15,0,1,0,"readwrite");
      JOY_RECV = new GBReg(0x150,31,0,1,0,"readwrite");
      JOY_TRANS = new GBReg(0x154,31,0,1,0,"readwrite");
      JOYSTAT = new GBReg(0x158,15,0,1,0,"readwrite");
   }
}

public class RegSect_keypad
{
   /// <summary>
   /// Key Status 2 R
   /// </summary>
   public GBReg KEYINPUT;
   /// <summary>
   /// Key Interrupt Control 2 R/W
   /// </summary>
   public GBReg KEYCNT;

   public RegSect_keypad() 
   {
      KEYINPUT = new GBReg(0x130,15,0,1,0,"readonly");
      KEYCNT = new GBReg(0x132,15,0,1,0,"readwrite");
   }
}

public class RegSect_system
{
   /// <summary>
   /// Interrupt Enable Register
   /// </summary>
   public GBReg IE;
   /// <summary>
   /// Interrupt Request Flags / IRQ Acknowledge
   /// </summary>
   public GBReg IF;
   /// <summary>
   /// Game Pak Waitstate Control
   /// </summary>
   public GBReg WAITCNT;
   /// <summary>
   /// Interrupt Master Enable Register
   /// </summary>
   public GBReg IME;
   /// <summary>
   /// Undocumented - Post Boot Flag
   /// </summary>
   public GBReg POSTFLG;
   /// <summary>
   /// Undocumented - Power Down Control
   /// </summary>
   public GBReg HALTCNT;
   /// <summary>
   /// Undocumented - Internal Memory Control (R/W) -- Mirrors of 4000800h (repeated each 64K)
   /// </summary>
   public GBReg MemCtrl;

   public RegSect_system() 
   {
      IE = new GBReg(0x200,15,0,1,0,"readwrite");
      IF = new GBReg(0x200,31,16,1,0,"readwrite");
      WAITCNT = new GBReg(0x204,15,0,1,0,"readwrite");
      IME = new GBReg(0x208,31,0,1,0,"readwrite");
      POSTFLG = new GBReg(0x300,7,0,1,0,"readwrite");
      HALTCNT = new GBReg(0x300,15,8,1,0,"writeonly");
      MemCtrl = new GBReg(0x800,31,0,1,0,"readwrite");
   }
}

public static class GBRegs
{
   public static RegSect_display Sect_display;
   public static RegSect_sound Sect_sound;
   public static RegSect_dma Sect_dma;
   public static RegSect_timer Sect_timer;
   public static RegSect_serial Sect_serial;
   public static RegSect_keypad Sect_keypad;
   public static RegSect_system Sect_system;

   public static byte[] data;
   public static byte[] rwmask;

   public static void reset()
   {
      Sect_display = new RegSect_display();
      Sect_sound = new RegSect_sound();
      Sect_dma = new RegSect_dma();
      Sect_timer = new RegSect_timer();
      Sect_serial = new RegSect_serial();
      Sect_keypad = new RegSect_keypad();
      Sect_system = new RegSect_system();

      data = new byte[2052];

      // DISPCNT at 0x000 = 0x0080;
      data[0] = 128 & 0xFF;
      // BG2RotScaleParDX at 0x020 = 256;
      data[32] = 256 & 0xFF;
      data[33] = (256 >> 8) & 0xFF;
      // BG2RotScaleParDMY at 0x024 = 256;
      data[36] = 16777216 & 0xFF;
      data[37] = (16777216 >> 8) & 0xFF;
      data[38] = (16777216 >> 16) & 0xFF;
      data[39] = (16777216 >> 24) & 0xFF;
      // BG3RotScaleParDX at 0x030 = 256;
      data[48] = 256 & 0xFF;
      data[49] = (256 >> 8) & 0xFF;
      // BG3RotScaleParDMY at 0x034 = 256;
      data[52] = 16777216 & 0xFF;
      data[53] = (16777216 >> 8) & 0xFF;
      data[54] = (16777216 >> 16) & 0xFF;
      data[55] = (16777216 >> 24) & 0xFF;
      // SOUNDBIAS at 0x088 = 0x0200;
      data[136] = 512 & 0xFF;
      data[137] = (512 >> 8) & 0xFF;

      rwmask = new byte[2052];

      // DISPCNT at 0x000 = 0xFFFFFFFF;
      rwmask[0] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[1] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[2] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[3] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // DISPSTAT at 0x004 = 0xFFFFFFFF;
      rwmask[4] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[5] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[6] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[7] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // BG0CNT at 0x008 = 0xDFFF;
      rwmask[8] = (byte)(0xDFFF & 0xFF);
      rwmask[9] = (byte)((0xDFFF >> 8) & 0xFF);
      rwmask[10] = (byte)((0xDFFF >> 16) & 0xFF);
      rwmask[11] = (byte)((0xDFFF >> 24) & 0xFF);
      // BG1CNT at 0x00A = 0xDFFF;
      rwmask[10] = (byte)(0xDFFF & 0xFF);
      rwmask[11] = (byte)((0xDFFF >> 8) & 0xFF);
      rwmask[12] = (byte)((0xDFFF >> 16) & 0xFF);
      rwmask[13] = (byte)((0xDFFF >> 24) & 0xFF);
      // BG2CNT at 0x00C = 0xFFFF;
      rwmask[12] = (byte)(0xFFFF & 0xFF);
      rwmask[13] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[14] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[15] = (byte)((0xFFFF >> 24) & 0xFF);
      // BG3CNT at 0x00E = 0xFFFF;
      rwmask[14] = (byte)(0xFFFF & 0xFF);
      rwmask[15] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[16] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[17] = (byte)((0xFFFF >> 24) & 0xFF);
      // BG0HOFS at 0x010 = 0x0;
      rwmask[16] = (byte)(0x0 & 0xFF);
      rwmask[17] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[18] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[19] = (byte)((0x0 >> 24) & 0xFF);
      // BG0VOFS at 0x012 = 0x0;
      rwmask[18] = (byte)(0x0 & 0xFF);
      rwmask[19] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[20] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[21] = (byte)((0x0 >> 24) & 0xFF);
      // BG1HOFS at 0x014 = 0x0;
      rwmask[20] = (byte)(0x0 & 0xFF);
      rwmask[21] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[22] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[23] = (byte)((0x0 >> 24) & 0xFF);
      // BG1VOFS at 0x016 = 0x0;
      rwmask[22] = (byte)(0x0 & 0xFF);
      rwmask[23] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[24] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[25] = (byte)((0x0 >> 24) & 0xFF);
      // BG2HOFS at 0x018 = 0x0;
      rwmask[24] = (byte)(0x0 & 0xFF);
      rwmask[25] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[26] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[27] = (byte)((0x0 >> 24) & 0xFF);
      // BG2VOFS at 0x01A = 0x0;
      rwmask[26] = (byte)(0x0 & 0xFF);
      rwmask[27] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[28] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[29] = (byte)((0x0 >> 24) & 0xFF);
      // BG3HOFS at 0x01C = 0x0;
      rwmask[28] = (byte)(0x0 & 0xFF);
      rwmask[29] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[30] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[31] = (byte)((0x0 >> 24) & 0xFF);
      // BG3VOFS at 0x01E = 0x0;
      rwmask[30] = (byte)(0x0 & 0xFF);
      rwmask[31] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[32] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[33] = (byte)((0x0 >> 24) & 0xFF);
      // BG2RotScaleParDX at 0x020 = 0x0;
      rwmask[32] = (byte)(0x0 & 0xFF);
      rwmask[33] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[34] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[35] = (byte)((0x0 >> 24) & 0xFF);
      // BG2RotScaleParDY at 0x024 = 0x0;
      rwmask[36] = (byte)(0x0 & 0xFF);
      rwmask[37] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[38] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[39] = (byte)((0x0 >> 24) & 0xFF);
      // BG2RefX at 0x028 = 0x0;
      rwmask[40] = (byte)(0x0 & 0xFF);
      rwmask[41] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[42] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[43] = (byte)((0x0 >> 24) & 0xFF);
      // BG2RefY at 0x02C = 0x0;
      rwmask[44] = (byte)(0x0 & 0xFF);
      rwmask[45] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[46] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[47] = (byte)((0x0 >> 24) & 0xFF);
      // BG3RotScaleParDX at 0x030 = 0x0;
      rwmask[48] = (byte)(0x0 & 0xFF);
      rwmask[49] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[50] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[51] = (byte)((0x0 >> 24) & 0xFF);
      // BG3RotScaleParDY at 0x034 = 0x0;
      rwmask[52] = (byte)(0x0 & 0xFF);
      rwmask[53] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[54] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[55] = (byte)((0x0 >> 24) & 0xFF);
      // BG3RefX at 0x038 = 0x0;
      rwmask[56] = (byte)(0x0 & 0xFF);
      rwmask[57] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[58] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[59] = (byte)((0x0 >> 24) & 0xFF);
      // BG3RefY at 0x03C = 0x0;
      rwmask[60] = (byte)(0x0 & 0xFF);
      rwmask[61] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[62] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[63] = (byte)((0x0 >> 24) & 0xFF);
      // WIN0H at 0x040 = 0x0;
      rwmask[64] = (byte)(0x0 & 0xFF);
      rwmask[65] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[66] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[67] = (byte)((0x0 >> 24) & 0xFF);
      // WIN0V at 0x044 = 0x0;
      rwmask[68] = (byte)(0x0 & 0xFF);
      rwmask[69] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[70] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[71] = (byte)((0x0 >> 24) & 0xFF);
      // WININ at 0x048 = 0x3F3F3F3F;
      rwmask[72] = (byte)(0x3F3F3F3F & 0xFF);
      rwmask[73] = (byte)((0x3F3F3F3F >> 8) & 0xFF);
      rwmask[74] = (byte)((0x3F3F3F3F >> 16) & 0xFF);
      rwmask[75] = (byte)((0x3F3F3F3F >> 24) & 0xFF);
      // MOSAIC at 0x04C = 0x0;
      rwmask[76] = (byte)(0x0 & 0xFF);
      rwmask[77] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[78] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[79] = (byte)((0x0 >> 24) & 0xFF);
      // BLDCNT at 0x050 = 0x1F1F3FFF;
      rwmask[80] = (byte)(0x1F1F3FFF & 0xFF);
      rwmask[81] = (byte)((0x1F1F3FFF >> 8) & 0xFF);
      rwmask[82] = (byte)((0x1F1F3FFF >> 16) & 0xFF);
      rwmask[83] = (byte)((0x1F1F3FFF >> 24) & 0xFF);
      // BLDY at 0x054 = 0x0;
      rwmask[84] = (byte)(0x0 & 0xFF);
      rwmask[85] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[86] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[87] = (byte)((0x0 >> 24) & 0xFF);
      // SOUND1CNT_L at 0x060 = 0xFFC0007F;
      rwmask[96] = (byte)(0xFFC0007F & 0xFF);
      rwmask[97] = (byte)((0xFFC0007F >> 8) & 0xFF);
      rwmask[98] = (byte)((0xFFC0007F >> 16) & 0xFF);
      rwmask[99] = (byte)((0xFFC0007F >> 24) & 0xFF);
      // SOUND1CNT_X at 0x064 = 0xFFFF4000;
      rwmask[100] = (byte)(0xFFFF4000 & 0xFF);
      rwmask[101] = (byte)((0xFFFF4000 >> 8) & 0xFF);
      rwmask[102] = (byte)((0xFFFF4000 >> 16) & 0xFF);
      rwmask[103] = (byte)((0xFFFF4000 >> 24) & 0xFF);
      // SOUND2CNT_L at 0x068 = 0xFFC0;
      rwmask[104] = (byte)(0xFFC0 & 0xFF);
      rwmask[105] = (byte)((0xFFC0 >> 8) & 0xFF);
      rwmask[106] = (byte)((0xFFC0 >> 16) & 0xFF);
      rwmask[107] = (byte)((0xFFC0 >> 24) & 0xFF);
      // SOUND2CNT_H at 0x06C = 0xFFFF4000;
      rwmask[108] = (byte)(0xFFFF4000 & 0xFF);
      rwmask[109] = (byte)((0xFFFF4000 >> 8) & 0xFF);
      rwmask[110] = (byte)((0xFFFF4000 >> 16) & 0xFF);
      rwmask[111] = (byte)((0xFFFF4000 >> 24) & 0xFF);
      // SOUND3CNT_L at 0x070 = 0xE00000E0;
      rwmask[112] = (byte)(0xE00000E0 & 0xFF);
      rwmask[113] = (byte)((0xE00000E0 >> 8) & 0xFF);
      rwmask[114] = (byte)((0xE00000E0 >> 16) & 0xFF);
      rwmask[115] = (byte)((0xE00000E0 >> 24) & 0xFF);
      // SOUND3CNT_X at 0x074 = 0xFFFF4000;
      rwmask[116] = (byte)(0xFFFF4000 & 0xFF);
      rwmask[117] = (byte)((0xFFFF4000 >> 8) & 0xFF);
      rwmask[118] = (byte)((0xFFFF4000 >> 16) & 0xFF);
      rwmask[119] = (byte)((0xFFFF4000 >> 24) & 0xFF);
      // SOUND4CNT_L at 0x078 = 0xFFFFFF00;
      rwmask[120] = (byte)(0xFFFFFF00 & 0xFF);
      rwmask[121] = (byte)((0xFFFFFF00 >> 8) & 0xFF);
      rwmask[122] = (byte)((0xFFFFFF00 >> 16) & 0xFF);
      rwmask[123] = (byte)((0xFFFFFF00 >> 24) & 0xFF);
      // SOUND4CNT_H at 0x07C = 0xFFFF40FF;
      rwmask[124] = (byte)(0xFFFF40FF & 0xFF);
      rwmask[125] = (byte)((0xFFFF40FF >> 8) & 0xFF);
      rwmask[126] = (byte)((0xFFFF40FF >> 16) & 0xFF);
      rwmask[127] = (byte)((0xFFFF40FF >> 24) & 0xFF);
      // SOUNDCNT_L at 0x080 = 0xFFFFFF77;
      rwmask[128] = (byte)(0xFFFFFF77 & 0xFF);
      rwmask[129] = (byte)((0xFFFFFF77 >> 8) & 0xFF);
      rwmask[130] = (byte)((0xFFFFFF77 >> 16) & 0xFF);
      rwmask[131] = (byte)((0xFFFFFF77 >> 24) & 0xFF);
      // SOUNDCNT_X at 0x084 = 0xFFFF00FF;
      rwmask[132] = (byte)(0xFFFF00FF & 0xFF);
      rwmask[133] = (byte)((0xFFFF00FF >> 8) & 0xFF);
      rwmask[134] = (byte)((0xFFFF00FF >> 16) & 0xFF);
      rwmask[135] = (byte)((0xFFFF00FF >> 24) & 0xFF);
      // SOUNDBIAS at 0x088 = 0xFFFFFFFF;
      rwmask[136] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[137] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[138] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[139] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // WAVE_RAM at 0x090 = 0xFFFFFFFF;
      rwmask[144] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[145] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[146] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[147] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // WAVE_RAM2 at 0x094 = 0xFFFFFFFF;
      rwmask[148] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[149] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[150] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[151] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // WAVE_RAM3 at 0x098 = 0xFFFFFFFF;
      rwmask[152] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[153] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[154] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[155] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // WAVE_RAM4 at 0x09C = 0xFFFFFFFF;
      rwmask[156] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[157] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[158] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[159] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // FIFO_A at 0x0A0 = 0x0;
      rwmask[160] = (byte)(0x0 & 0xFF);
      rwmask[161] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[162] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[163] = (byte)((0x0 >> 24) & 0xFF);
      // FIFO_B at 0x0A4 = 0x0;
      rwmask[164] = (byte)(0x0 & 0xFF);
      rwmask[165] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[166] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[167] = (byte)((0x0 >> 24) & 0xFF);
      // DMA0SAD at 0xB0 = 0x0;
      rwmask[176] = (byte)(0x0 & 0xFF);
      rwmask[177] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[178] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[179] = (byte)((0x0 >> 24) & 0xFF);
      // DMA0DAD at 0xB4 = 0x0;
      rwmask[180] = (byte)(0x0 & 0xFF);
      rwmask[181] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[182] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[183] = (byte)((0x0 >> 24) & 0xFF);
      // DMA0CNT_L at 0xB8 = 0xF7E00000;
      rwmask[184] = (byte)(0xF7E00000 & 0xFF);
      rwmask[185] = (byte)((0xF7E00000 >> 8) & 0xFF);
      rwmask[186] = (byte)((0xF7E00000 >> 16) & 0xFF);
      rwmask[187] = (byte)((0xF7E00000 >> 24) & 0xFF);
      // DMA1SAD at 0xBC = 0x0;
      rwmask[188] = (byte)(0x0 & 0xFF);
      rwmask[189] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[190] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[191] = (byte)((0x0 >> 24) & 0xFF);
      // DMA1DAD at 0xC0 = 0x0;
      rwmask[192] = (byte)(0x0 & 0xFF);
      rwmask[193] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[194] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[195] = (byte)((0x0 >> 24) & 0xFF);
      // DMA1CNT_L at 0xC4 = 0xF7E00000;
      rwmask[196] = (byte)(0xF7E00000 & 0xFF);
      rwmask[197] = (byte)((0xF7E00000 >> 8) & 0xFF);
      rwmask[198] = (byte)((0xF7E00000 >> 16) & 0xFF);
      rwmask[199] = (byte)((0xF7E00000 >> 24) & 0xFF);
      // DMA2SAD at 0xC8 = 0x0;
      rwmask[200] = (byte)(0x0 & 0xFF);
      rwmask[201] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[202] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[203] = (byte)((0x0 >> 24) & 0xFF);
      // DMA2DAD at 0xCC = 0x0;
      rwmask[204] = (byte)(0x0 & 0xFF);
      rwmask[205] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[206] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[207] = (byte)((0x0 >> 24) & 0xFF);
      // DMA2CNT_L at 0xD0 = 0xF7E00000;
      rwmask[208] = (byte)(0xF7E00000 & 0xFF);
      rwmask[209] = (byte)((0xF7E00000 >> 8) & 0xFF);
      rwmask[210] = (byte)((0xF7E00000 >> 16) & 0xFF);
      rwmask[211] = (byte)((0xF7E00000 >> 24) & 0xFF);
      // DMA3SAD at 0xD4 = 0x0;
      rwmask[212] = (byte)(0x0 & 0xFF);
      rwmask[213] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[214] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[215] = (byte)((0x0 >> 24) & 0xFF);
      // DMA3DAD at 0xD8 = 0x0;
      rwmask[216] = (byte)(0x0 & 0xFF);
      rwmask[217] = (byte)((0x0 >> 8) & 0xFF);
      rwmask[218] = (byte)((0x0 >> 16) & 0xFF);
      rwmask[219] = (byte)((0x0 >> 24) & 0xFF);
      // DMA3CNT_L at 0xDC = 0xFFE00000;
      rwmask[220] = (byte)(0xFFE00000 & 0xFF);
      rwmask[221] = (byte)((0xFFE00000 >> 8) & 0xFF);
      rwmask[222] = (byte)((0xFFE00000 >> 16) & 0xFF);
      rwmask[223] = (byte)((0xFFE00000 >> 24) & 0xFF);
      // TM0CNT_L at 0x100 = 0xFFFFFFFF;
      rwmask[256] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[257] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[258] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[259] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // TM1CNT_L at 0x104 = 0xFFFFFFFF;
      rwmask[260] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[261] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[262] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[263] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // TM2CNT_L at 0x108 = 0xFFFFFFFF;
      rwmask[264] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[265] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[266] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[267] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // TM3CNT_L at 0x10C = 0xFFFFFFFF;
      rwmask[268] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[269] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[270] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[271] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // SIODATA32 at 0x120 = 0xFFFFFFFF;
      rwmask[288] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[289] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[290] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[291] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // SIOMULTI1 at 0x122 = 0xFFFF;
      rwmask[290] = (byte)(0xFFFF & 0xFF);
      rwmask[291] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[292] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[293] = (byte)((0xFFFF >> 24) & 0xFF);
      // SIOMULTI2 at 0x124 = 0xFFFF;
      rwmask[292] = (byte)(0xFFFF & 0xFF);
      rwmask[293] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[294] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[295] = (byte)((0xFFFF >> 24) & 0xFF);
      // SIOMULTI3 at 0x126 = 0xFFFF;
      rwmask[294] = (byte)(0xFFFF & 0xFF);
      rwmask[295] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[296] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[297] = (byte)((0xFFFF >> 24) & 0xFF);
      // SIOCNT at 0x128 = 0xFFFF;
      rwmask[296] = (byte)(0xFFFF & 0xFF);
      rwmask[297] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[298] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[299] = (byte)((0xFFFF >> 24) & 0xFF);
      // SIOMLT_SEND at 0x12A = 0xFFFF;
      rwmask[298] = (byte)(0xFFFF & 0xFF);
      rwmask[299] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[300] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[301] = (byte)((0xFFFF >> 24) & 0xFF);
      // KEYINPUT at 0x130 = 0xFFFF;
      rwmask[304] = (byte)(0xFFFF & 0xFF);
      rwmask[305] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[306] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[307] = (byte)((0xFFFF >> 24) & 0xFF);
      // KEYCNT at 0x132 = 0xFFFF;
      rwmask[306] = (byte)(0xFFFF & 0xFF);
      rwmask[307] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[308] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[309] = (byte)((0xFFFF >> 24) & 0xFF);
      // RCNT at 0x134 = 0xFFFF;
      rwmask[308] = (byte)(0xFFFF & 0xFF);
      rwmask[309] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[310] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[311] = (byte)((0xFFFF >> 24) & 0xFF);
      // IR at 0x136 = 0xFFFF;
      rwmask[310] = (byte)(0xFFFF & 0xFF);
      rwmask[311] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[312] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[313] = (byte)((0xFFFF >> 24) & 0xFF);
      // JOYCNT at 0x140 = 0xFFFF;
      rwmask[320] = (byte)(0xFFFF & 0xFF);
      rwmask[321] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[322] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[323] = (byte)((0xFFFF >> 24) & 0xFF);
      // JOY_RECV at 0x150 = 0xFFFFFFFF;
      rwmask[336] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[337] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[338] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[339] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // JOY_TRANS at 0x154 = 0xFFFFFFFF;
      rwmask[340] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[341] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[342] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[343] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // JOYSTAT at 0x158 = 0xFFFF;
      rwmask[344] = (byte)(0xFFFF & 0xFF);
      rwmask[345] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[346] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[347] = (byte)((0xFFFF >> 24) & 0xFF);
      // IE at 0x200 = 0xFFFFFFFF;
      rwmask[512] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[513] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[514] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[515] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // WAITCNT at 0x204 = 0xFFFF;
      rwmask[516] = (byte)(0xFFFF & 0xFF);
      rwmask[517] = (byte)((0xFFFF >> 8) & 0xFF);
      rwmask[518] = (byte)((0xFFFF >> 16) & 0xFF);
      rwmask[519] = (byte)((0xFFFF >> 24) & 0xFF);
      // IME at 0x208 = 0xFFFFFFFF;
      rwmask[520] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[521] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[522] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[523] = (byte)((0xFFFFFFFF >> 24) & 0xFF);
      // POSTFLG at 0x300 = 0xFF;
      rwmask[768] = (byte)(0xFF & 0xFF);
      rwmask[769] = (byte)((0xFF >> 8) & 0xFF);
      rwmask[770] = (byte)((0xFF >> 16) & 0xFF);
      rwmask[771] = (byte)((0xFF >> 24) & 0xFF);
      // MemCtrl at 0x800 = 0xFFFFFFFF;
      rwmask[2048] = (byte)(0xFFFFFFFF & 0xFF);
      rwmask[2049] = (byte)((0xFFFFFFFF >> 8) & 0xFF);
      rwmask[2050] = (byte)((0xFFFFFFFF >> 16) & 0xFF);
      rwmask[2051] = (byte)((0xFFFFFFFF >> 24) & 0xFF);

   }
}
