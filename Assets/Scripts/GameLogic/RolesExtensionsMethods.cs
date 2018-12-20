namespace GameLogic.Units
{
	public static class RolesExtensionMethods
	{
		public static bool isHero(this Roles role)
		{
			if (role == Roles.FIGHTER || role == Roles.ROGUE || role == Roles.MAGE)
				return true;
			else
				return false;
		}
	}
}

