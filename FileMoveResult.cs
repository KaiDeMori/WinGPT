namespace WinGPT;

public enum FileMoveResult {
   Success,
   SuccessWithRename,
   FileDoesNotExist,
   CategoryDoesNotExist,
   CategoryExistsButShouldNot,
   UnknownError
}