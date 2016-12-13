
# coding: utf-8

# In[5]:

import pandas as pd
from sql_server import pyodbc
import matplotlib.pyplot as plt


# In[6]:

df = pd.read_csv('sampleData.csv', encoding='latin-1')


# In[148]:

df.head()


# ## Renaming column

# In[149]:

df = df.rename(columns = {'dateOfSale':'date_of_sale'})
df.head()


# # Parsing dates

# In[150]:

df['date_of_sale'] = pd.to_datetime(df['date_of_sale'], format='%d/%m/%Y')
df.head()


# # Full market values parsing

# In[151]:

def findFull(row):
    return row.find('**') != -1
df['not_full_market'] = df['price'].apply(lambda row: findFull(row))


# In[152]:

df.head()


# ## Parsing price

# In[153]:

def parsePrice(row):
    str_a = row[3:]
    str_a = str_a.replace(' ', '')
    str_a = str_a.replace('**', '')
    str_a = str_a.replace(',','')
    
    return float(str_a)

df['price'] = df['price'].map(lambda row: parsePrice(row))
df.head()


# # Inserting to DB

# In[1]:

# Database Connection Info
db="C:\Users\User\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB"

try:
    conn = p.connect(db)
except Exception:
    print"No Connection"




# In[ ]:



