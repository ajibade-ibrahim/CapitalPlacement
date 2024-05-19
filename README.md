# CapitalPlacement
Brief Information
-----------------
I created different model classes including Program, Question, Answer, and Application.
All entities are in the same container with the ProgramId as partition Key so that they can be retrieved together.
Due to the instruction to store the different types of questions, I separated the Program entity from the Question entity in order to create CRUD endpoints specific to questions.
Also, by separating Program from Questions, the access pattern, "Get All Programs" (which is probably the most used), will be more efficient as less data will be transferred.
I used EntityFramework to create a context class (ProgramContext) and I also created a service class (ProgramService) to encapsulate the required database operations.
For the Question endpoints, I required ProgramId just for the sake of representing the Program-Question heirarchy. The endpoints can be structured not to require the ProgramId.
The Update operations implemented perform a full object ovewrites, not patching.
Question information is duplicated within Answers to avoid the use of joins during retrieval. Any updates to Questions can be propagated to the Answers using the ChangeFeed.
QuestionType is represented using the Enum number when returned via the API but is actually stored as string in the database.
I skipped validation for individual QuestionTypes in the QuestionController due to time.
The ConnectionString information is included in the appConfig file but ideally this can be retrieved via a Key/Secret service like the Azure Key Vault.
The ProgramService class contains some methods which are unused in the controllers but which were used during practice with hard-coded values (these should've been deleted ideally).
