namespace DataAccess.Enums;

// enum value data type can be used for small sets of data which have the text part
// such as Master, Senior and Junior, and the value part such as 1 assigned to Master,
// 2 automatically assigned to Senior and 3 automatically assigned to Junior.
// If there is no value assignment, the first element in the enum starts with the value 0
// and other elements' values are automatically assigned with the last element's value incremented by 1.
public enum Statuses
{
    Master = 1,
    Senior,
    Junior
}
