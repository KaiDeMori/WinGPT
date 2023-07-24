namespace WinGPT;

public enum FileUpdateLocationResult {
   Success,
   SuccessWithRename, // file with the same name exists at the destination and a timestamp was added
   FileDoesNotExist,
   CategoryDoesNotExist,
   CategoryExistsButShouldNot,
   FileCouldNotBeCreated,
   FileCouldNotBeDeleted,
   FunctionParametersFaulty,
   UserAborted,
   UnknownError,
}