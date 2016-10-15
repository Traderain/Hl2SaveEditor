/* https://github.com/LestaD/SourceEngine2007/blob/43a5c90a5ada1e69ca044595383be67f40b33c61/src_main/public/saverestoretypes.h#L323
 * https://github.com/LestaD/SourceEngine2007/blob/43a5c90a5ada1e69ca044595383be67f40b33c61/src_main/engine/host_saverestore.cpp
 * https://github.com/LestaD/SourceEngine2007/blob/43a5c90a5ada1e69ca044595383be67f40b33c61/src_main/gameui/BaseSaveGameDialog.cpp
 * https://github.com/LestaD/SourceEngine2007/blob/43a5c90a5ada1e69ca044595383be67f40b33c61/src_main/tier1/lzss.cpp
 * TODO: Implement these
 * 
 * Decompress the file with CLZSS -> Read header -> Parse files.
 * 
 * 
 */




using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Listsave
{
    public class Flag
    {
        public string Ticks { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }

        public Flag(int t, float s, string type)
        {
            Ticks = t.ToString();
            Time = s.ToString(CultureInfo.InvariantCulture) + "s";
            Type = type;
        }
    }

    public class Listsave
    {

        public int SAVEGAME_MAPNAME_LEN = 32;
        public int SAVEGAME_COMMENT_LEN = 80;
        public int SAVEGAME_ELAPSED_LEN = 32;
        public int SECTION_MAGIC_NUMBER = 0x54541234;
        public int SECTION_VERSION_NUMBER = 2;

        unsafe struct GAME_HEADER
        {

            fixed char mapName[32];
            fixed char comment[80];
            int mapCount;       // the number of map state files in the save file.  This is usually number of maps * 3 (.hl1, .hl2, .hl3 files)
            fixed char originMapName[32];
            fixed char landmark[256];
        };

        unsafe struct SAVE_HEADER
        {
            int saveId;
            int version;
            int skillLevel;
            int connectionCount;
            int lightStyleCount;
            int mapVersion;
            float time;
            fixed char mapName[32];
            fixed char skyName[32];
        };

        public static string Chaptername(int chapter)
        {
            #region MapSwitch
            switch (chapter)
            {
                case 0: return "Point Insertion";
                case 1: return "A Red Letter Day";
                case 2: return "Route Kanal";
                case 3: return "Water Hazard";
                case 4: return "Black Mesa East";
                case 5: return "We don't go to Ravenholm";
                case 6: return "Highway 17";
                case 7: return "Sandtraps";
                case 8: return "Nova Prospekt";
                case 9: return "Entanglement";
                case 10: return "Anticitizen One";
                case 11: return "Follow Freeman!";
                case 12: return "Our Benefactors";
                case 13: return "Dark Energy";
                default: return "Mod/Unknown";
            }
            #endregion
        }
        public static SaveFile ParseFile(string file)
        {
            var result = new SaveFile();
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                result.Header = (Encoding.ASCII.GetString(br.ReadBytes(sizeof(int))));
                result.SaveVersion = br.ReadInt32();
                result.Size = br.ReadInt32();
                result.TokenCount = br.ReadInt32();
                result.Tokensize = br.ReadInt32();

                return new SaveFile(); //TODO: return
            }
        }
    }

    [Serializable]
    public class SaveFile
    {
       public string FileName { get; set; }
       public string Header { get; set; }
       public int SaveVersion { get; set; }
       public int Size { get; set; }
       public int Tokensize { get; set; }
       public int TokenCount { get; set; }
       public List<Token> Tokentable { get; set;}
       public string Chapter { get; set; }
       public string Map { get; set; }
        
    }

    public class Token
    {
        public string Name;
        public string Value;

        public Token(string n, string v)
        {
            this.Name = n;
            this.Value = v;
        }
    }
}
/*
 * int CSaveRestore::SaveReadHeader( FileHandle_t pFile, GAME_HEADER *pHeader, int readGlobalState )
{
	int					i, tag, size, tokenCount, tokenSize;
	char				*pszTokenList;
	CSaveRestoreData	*pSaveData = NULL;

	if( g_pSaveRestoreFileSystem->Read( &tag, sizeof(int), pFile ) != sizeof(int) )
		return 0;

	if ( tag != MAKEID('J','S','A','V') )
	{
		Warning( "Can't load saved game, incorrect FILEID\n" );
		return 0;
	}
		
	if ( g_pSaveRestoreFileSystem->Read( &tag, sizeof(int), pFile ) != sizeof(int) )
		return 0;

	if ( tag != SAVEGAME_VERSION )				// Enforce version for now
	{
		Warning( "Can't load saved game, incorrect version (got %i expecting %i)\n", tag, SAVEGAME_VERSION );
		return 0;
	}

	if ( g_pSaveRestoreFileSystem->Read( &size, sizeof(int), pFile ) != sizeof(int) )
		return 0;

	if ( g_pSaveRestoreFileSystem->Read( &tokenCount, sizeof(int), pFile ) != sizeof(int) )
		return 0;

	if ( g_pSaveRestoreFileSystem->Read( &tokenSize, sizeof(int), pFile ) != sizeof(int) ) 
		return 0;

	// At this point we must clean this data up if we fail!
	void *pSaveMemory = SaveAllocMemory( sizeof(CSaveRestoreData) + tokenSize + size, sizeof(char) );
	if ( !pSaveMemory )
	{
		return 0;
	}

	pSaveData = MakeSaveRestoreData( pSaveMemory );

	pSaveData->levelInfo.connectionCount = 0;

	pszTokenList = (char *)(pSaveData + 1);

	if ( tokenSize > 0 )
	{
		if ( g_pSaveRestoreFileSystem->Read( pszTokenList, tokenSize, pFile ) != tokenSize )
		{
			Finish( pSaveData );
			return 0;
		}

		pSaveMemory = SaveAllocMemory( tokenCount, sizeof(char *), true );
		if ( !pSaveMemory )
		{
			Finish( pSaveData );
			return 0;
		}

		pSaveData->InitSymbolTable( (char**)pSaveMemory, tokenCount );

		// Make sure the token strings pointed to by the pToken hashtable.
		for( i=0; i<tokenCount; i++ )
		{
			if ( *pszTokenList )
			{
				Verify( pSaveData->DefineSymbol( pszTokenList, i ) );
			}
			while( *pszTokenList++ );				// Find next token (after next null)
		}
	}
	else
	{
		pSaveData->InitSymbolTable( NULL, 0 );
	}


	pSaveData->levelInfo.fUseLandmark = false;
	pSaveData->levelInfo.time = 0;

	// pszTokenList now points after token data
	pSaveData->Init( pszTokenList, size ); 
	if ( g_pSaveRestoreFileSystem->Read( pSaveData->GetBuffer(), size, pFile ) != size )
	{
		Finish( pSaveData );
		return 0;
	}
	

    //return m_pServerGameDLL->SaveReadFields( s, c, v, d, t, i );
	serverGameDLL->SaveReadFields( pSaveData, "GameHeader", pHeader, NULL, GAME_HEADER::m_DataMap.dataDesc, GAME_HEADER::m_DataMap.dataNumFields );
	
    
    if ( g_szMapLoadOverride[0] )
	{
		V_strncpy( pHeader->mapName, g_szMapLoadOverride, sizeof( pHeader->mapName ) );
		g_szMapLoadOverride[0] = 0;
	}

	if ( readGlobalState )
	{
		serverGameDLL->RestoreGlobalState( pSaveData );
	}

	Finish( pSaveData );
    /*
     * void CSaveRestore::Finish( CSaveRestoreData *save )
        {
        	char **pTokens = save->DetachSymbolTable();
        	if ( pTokens )
        		SaveFreeMemory( pTokens );
        
        	entitytable_t *pEntityTable = save->DetachEntityTable();
        	if ( pEntityTable )
        		SaveFreeMemory( pEntityTable );
        
        	save->PurgeEntityHash();
        	SaveFreeMemory( save );
        
        
        	g_ServerGlobalVariables.pSaveData = NULL;
        }
      /*
      

	if ( pHeader->mapCount == 0 )
	{
		if ( g_pSaveRestoreFileSystem->Read( &pHeader->mapCount, sizeof(pHeader->mapCount), pFile ) != sizeof(pHeader->mapCount) )
			return 0;
	}

	return 1;
}
 */
