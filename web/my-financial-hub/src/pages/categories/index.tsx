import { useEffect, useState } from 'react';

import { useApisContext } from '../../commom/contexts/api-context';
import { useDeleteCategory, useGetCategories } from '../../commom/hooks/categories-hooks';

import { Category, defaultCategory } from '../../commom/interfaces/category';

import CategoryForm from '../../commom/components/categories/form/category-form';
import FormSelect from '../../commom/components/forms/form-select';
import SelectOption from '../../commom/components/forms/form-select/types/select-option';

//TODO create APIFormSelect
export default function CategoriesPage() {
  const { categoriesApi } = useApisContext();
  
  const [categories,setCategories] = useState<Category[]>([]);
  const [categoryOptions, setCategoryOptions] = useState<SelectOption[]>([]);
  const [selectedCategory, setCategory] = useState<Category>();
  const [isLoading, setLoading] = useState(false);

  const getCategories = async function(){
    setLoading(true);

    const categories = await useGetCategories(categoriesApi);
    setCategories(categories);

    setLoading(false);
  };

  const selectCategory = function(option?: SelectOption){
    console.log(option);
    if(!option){
      setCategory(defaultCategory);
      return;
    }

    const foundCategories = categories.filter(c => c.id == option.value);
    if(foundCategories.length > 0){
      console.log(foundCategories[0]);
      setCategory(foundCategories[0]);
    }
  };

  const submitCategory = async function(category: Category){
    const foundCategories = categories.filter(c => c.id == category.id);

    if(foundCategories.length > 0){
      const index = categories.findIndex(obj => obj.id == category.id);
      categories[index] = category;
      setCategories(categories);
    }else{
      setCategories([...categories, category]);
    }
  };

  const deleteCategory = async function(id?: string){
    if(id){
      setLoading(true);
      
      await useDeleteCategory(id,categoriesApi);
      setCategories(categories.filter(x => x.id !== id));

      setLoading(false);
    }
  };

  useEffect(
    () => {
      getCategories();
    }
    ,[]
  );
  
  // TODO : recieve the 'convert' method on FormSelectProps (?)
  useEffect(
    ()=>{
      setCategoryOptions(
        categories.map(
          cat => (
            { 
              value: cat.id ?? '', 
              label: cat.name
            }
          )
        )
      );
    }
    ,[categories]
  );

  return (
    <div className='container'>
      <CategoryForm formData={selectedCategory} onSubmit={submitCategory}/>
      <FormSelect 
        placeholder='Select a category'
        disabled={isLoading} 
        options={categoryOptions} 
        onChangeOption={selectCategory} 
        onDeleteOption={(e) => deleteCategory(e)}
      />
    </div>
  );
}