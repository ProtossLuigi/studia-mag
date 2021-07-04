library(RWeka)
library(datasets)
library(rattle.data)
library(mlbench)
library(ggplot2)
library(caret)
library(C50)
library(MLmetrics)

data("iris")
data("Glass")

dfs <- c(iris, wine, Glass)

cfactors <- c(0.05, 0.1, 0.15, 0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5)
minno <- c(2:11)
logc <- c(FALSE, TRUE)

inTrain <- createFolds(
    y = Glass$Type,
    k = 5,
    list = FALSE
)

acc <- c()
prec <- c()
rec <- c()
f1 <- c()
for (i in c(1:5)){
    training <- Glass[ inTrain != i,]
    testing  <- Glass[ inTrain == i,]
    m <- C5.0(Type ~ ., data=training)
    y_true <- testing$Type
    y_pred <- predict(m, newdata=testing)
    acc <- append(acc, Accuracy(y_pred, y_true))
    prec <- append(prec, Precision(y_true, y_pred, positive = NULL))
    rec <- append(rec, Recall(y_true, y_pred, positive = NULL))
    f1 <- append(f1, F1_Score(y_true, y_pred, positive = NULL))
}

print(mean(acc))
print(mean(prec))
print(mean(rec))
print(mean(f1))

# inTrain <- createDataPartition(
#     y = wine$Type,
#     p = .75,
#     list = FALSE
# )
# training <- wine[ inTrain,]
# testing  <- wine[-inTrain,]

# inTrain <- createDataPartition(
#     y = Glass$Type,
#     p = .75,
#     list = FALSE
# )
# training <- Glass[ inTrain,]
# testing  <- Glass[-inTrain,]

# m <- C5.0(Type ~ ., data=training)
# y_true <- testing$Type
# y_pred <- predict(m, newdata=testing)
# # print(y_true)
# # print(y_pred)

# Accuracy(y_pred, y_true)
# Precision(y_true, y_pred, positive = NULL)
# Recall(y_true, y_pred, positive = NULL)
# F1_Score(y_true, y_pred, positive = NULL)


# png("C45_glass_heatmap.png", width = 720, height = 720)

# irisv <- c()
# for (cfactor in cfactors) {
#     model <- J48(Species ~ ., data=training, control=Weka_control(C=cfactor))
#     e <- evaluate_Weka_classifier(model, newdata=testing, class=TRUE)
#     irisv <- append(irisv, e$details["pctCorrect"])
# }

# inTrain <- createDataPartition(
#     y = wine$Type,
#     p = .75,
#     list = FALSE
# )
# training <- wine[ inTrain,]
# testing  <- wine[-inTrain,]

# winev <- c()
# for (cfactor in cfactors) {
#     model <- J48(Type ~ ., data=training, control=Weka_control(C=cfactor))
#     e <- evaluate_Weka_classifier(model, newdata=testing, class=TRUE)
#     winev <- append(winev, e$details["pctCorrect"])
# }

# inTrain <- createDataPartition(
#     y = Glass$Type,
#     p = .75,
#     list = FALSE
# )
# training <- Glass[ inTrain,]
# testing  <- Glass[-inTrain,]

# glassv <- c()
# for (cfactor in cfactors) {
#     model <- J48(Type ~ ., data=training, control=Weka_control(C=cfactor))
#     e <- evaluate_Weka_classifier(model, newdata=testing, class=TRUE)
#     glassv <- append(glassv, e$details["pctCorrect"])
# }

# irisv <- c()
# for (i in minno) {
#     model <- J48(Species ~ ., data=training, control=Weka_control(M=i))
#     e <- evaluate_Weka_classifier(model, newdata=testing, class=TRUE)
#     irisv <- append(irisv, e$details["pctCorrect"])
# }

# inTrain <- createDataPartition(
#     y = wine$Type,
#     p = .75,
#     list = FALSE
# )
# training <- wine[ inTrain,]
# testing  <- wine[-inTrain,]

# winev <- c()
# for (i in minno) {
#     model <- J48(Type ~ ., data=training, control=Weka_control(M=i))
#     e <- evaluate_Weka_classifier(model, newdata=testing, class=TRUE)
#     winev <- append(winev, e$details["pctCorrect"])
# }

# inTrain <- createDataPartition(
#     y = Glass$Type,
#     p = .75,
#     list = FALSE
# )
# training <- Glass[ inTrain,]
# testing  <- Glass[-inTrain,]

# glassv <- c()
# for (i in minno) {
#     model <- J48(Type ~ ., data=training, control=Weka_control(M=i))
#     e <- evaluate_Weka_classifier(model, newdata=testing, class=TRUE)
#     glassv <- append(glassv, e$details["pctCorrect"])
# }

# inTrain <- createDataPartition(
#     y = iris$Species,
#     p = .75,
#     list = FALSE
# )
# training <- iris[ inTrain,]
# testing  <- iris[-inTrain,]

# irisv <- c()
# for (i in logc) {
#     m <- C5.0(Species ~ ., data=training, control=C5.0Control(noGlobalPruning=i))
#     y_true <- testing$Species
#     y_pred <- predict(m, newdata=testing)
#     irisv <- append(irisv, Accuracy(y_pred, y_true))
# }

# inTrain <- createDataPartition(
#     y = wine$Type,
#     p = .75,
#     list = FALSE
# )
# training <- wine[ inTrain,]
# testing  <- wine[-inTrain,]

# winev <- c()
# for (i in logc) {
#     m <- C5.0(Type ~ ., data=training, control=C5.0Control(noGlobalPruning=i))
#     y_true <- testing$Type
#     y_pred <- predict(m, newdata=testing)
#     winev <- append(winev, Accuracy(y_pred, y_true))
# }

# inTrain <- createDataPartition(
#     y = Glass$Type,
#     p = .75,
#     list = FALSE
# )
# training <- Glass[ inTrain,]
# testing  <- Glass[-inTrain,]

# glassv <- c()
# for (i in logc) {
#     m <- C5.0(Type ~ ., data=training, control=C5.0Control(noGlobalPruning=i))
#     y_true <- testing$Type
#     y_pred <- predict(m, newdata=testing)
#     glassv <- append(glassv, Accuracy(y_pred, y_true))
# }

# png("C50_ngp.png", width = 720, height = 720)

# plot(irisv~logc , type="b" , bty="l" , xlab="noGlobalPruning" , ylab="acc" , col=rgb(0.2,0.4,0.1,0.7) , lwd=3 , pch=17, ylim=c(0,1))
# lines(winev~logc , col=rgb(0.8,0.4,0.1,0.7) , lwd=3 , pch=19 , type="b" )
# lines(glassv~logc , col=rgb(0.3,0.4,0.8,0.7) , lwd=3 , pch=15 , type="b" )
 
# # Add a legend
# legend("bottomleft", 
#   legend = c("iris", "wine", "glass"), 
#   col = c(rgb(0.2,0.4,0.1,0.7), 
#   rgb(0.8,0.4,0.1,0.7),
#   rgb(0.3,0.4,0.8,0.7)), 
#   pch = c(17,19,15), 
#   bty = "n", 
#   pt.cex = 2, 
#   cex = 1.2, 
#   text.col = "black", 
#   horiz = F , 
#   inset = c(0.1, 0.1))

# model <- J48(Species ~ ., data=training)
# # plot(model)
# e <- evaluate_Weka_classifier(model, newdata=testing, class=TRUE)
# print(e$details)