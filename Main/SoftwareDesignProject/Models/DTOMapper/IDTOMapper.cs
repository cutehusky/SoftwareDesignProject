namespace SoftwareDesignProject.Models.DTOMapper;

public interface IDTOMapper<TEntityModel, TDTO>
{
    public TDTO ConvertTo(TEntityModel from);
    public TEntityModel ConvertFrom(TDTO from);

    public void CopyToEntity(TEntityModel target, TDTO source);
}