namespace NutritionsApi.Exceptions;

public class NotFoundException(string resourceName, object key)
    : ServiceException($"{resourceName} with id {key} was not found.", 404, "Object not found");