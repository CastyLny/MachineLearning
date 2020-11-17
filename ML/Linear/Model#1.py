import numpy as np
import os
import sys


import pandas as pd
from six.moves import urllib
import matplotlib.pyplot as plt
from IPython.display import clear_output

import tensorflow as tf



dfTrain = pd.read_csv('train.csv')
dfEvalRaw = pd.read_csv('eval.csv')

dfTest = pd.read_csv('test.csv')

yTrain = dfTrain.pop('output')
yEval = dfEvalRaw.pop('output')
yTest = dfTest.pop('output')

print(dfTrain)

features = [str("xa"), str("ax")]
featureColumn = []
for feature in features:
    featureColumn.append(tf.feature_column.numeric_column(feature,dtype=tf.float16))




def makeInputFunction(data_df, label_df, numsEpoch = 10, shuffle = True, batchSize = 20):
    def inputFunction():
        ds = tf.data.Dataset.from_tensor_slices((dict(data_df), label_df))
        if shuffle:
            ds.shuffle(1000)
        ds = ds.batch(batchSize).repeat(numsEpoch)
        return ds
    return inputFunction
print(yTrain)
trainInputFn = makeInputFunction(dfTrain, yTrain)
evalInputFn = makeInputFunction(dfEvalRaw, yEval, shuffle = False, numsEpoch=1)

testInputFn = makeInputFunction(dfTest, yTest, shuffle = False, numsEpoch=1)

linear_est = tf.estimator.LinearRegressor(feature_columns=featureColumn)
linear_est.train(trainInputFn)
result = linear_est.evaluate(evalInputFn)

clear_output()
print(result)
predictArr = list(linear_est.predict(evalInputFn))
result = pd.DataFrame({'predict':[pred['predictions'][0] for pred in predictArr],'real': yEval})
plt.figure()
result.plot()
plt.show()