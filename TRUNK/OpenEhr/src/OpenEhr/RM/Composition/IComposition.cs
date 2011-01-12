/// <licence>
///                                                                                    
///  Copyright (C) 2006  Ocean Informatics Pty Ltd, Australia                          
///                                                                                    
///  This program is free software; you can redistribute it and/or modify              
///  it under the terms of the GNU General Public License as published by              
///  the Free Software Foundation; either version 2 of the License, or                 
///  (at your option) any later version.                                               
///                                                                                    
///  This program is distributed in the hope that it will be useful,                   
///  but WITHOUT ANY WARRANTY; without even the implied warranty of                    
///  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                     
///  GNU General Public License for more details.                                      
///                                                                                    
///  You should have received a copy of the GNU General Public License                 
///  along with this program; if not, write to the Free Software                       
///  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA    
///                                                                                    
///  You may obtain a copy of the License at                                           
///  http://www.gnu.org/licenses/gpl.txt    
///                                           
/// </licence>

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEhr.RM.Composition
{
    //public interface IComposition : Common.ChangeControl.IVersionable
    public interface IComposition : Common.Archetyped.ILocatable
    {
        AssumedTypes.List<Content.ContentItem> Content
        {
            get;
            //set;
        }

        //OpenEhrV1.Support.Identification.UidBasedId Uid
        //{
        //    get;
        //}

        DataTypes.Text.DvCodedText Category
        {
            get;
            //set;
        }

        DataTypes.Text.CodePhrase Territory
        {
            get;
            //set;
        }

        EventContext Context
        {
            get;
            //set;
        }

        Common.Generic.PartyProxy Composer
        {
            get;
            //set;
        }

        bool IsPersistent();
    }
}
